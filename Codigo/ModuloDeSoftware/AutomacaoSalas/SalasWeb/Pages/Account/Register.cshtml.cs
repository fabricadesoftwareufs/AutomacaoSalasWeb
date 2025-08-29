using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.ViewModel;
using SalasWeb.Data;
using SalasWeb.Models.ViewModels;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utils;

namespace SalasWeb.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IOrganizacaoService organizacaoService,
            IUsuarioService usuarioService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _organizacaoService = organizacaoService;
            _usuarioService = usuarioService;
            _emailSender = emailSender;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public SelectList Organizacoes { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            LoadOrganizacoes();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            LoadOrganizacoes();

            if (ModelState.IsValid)
            {
                if (!Methods.ValidarCpf(Input.Cpf))
                {
                    ModelState.AddModelError(nameof(Input.Cpf), "CPF inválido!");
                    return Page();
                }

                if (!Methods.ValidarDataNascimento(Input.BirthDate))
                {
                    ModelState.AddModelError(nameof(Input.BirthDate), "Data de nascimento inválida.");
                    return Page();
                }

                var cpfLimpo = Methods.CleanString(Input.Cpf);

                // Verificar se já existe usuário com esse CPF no Identity
                var existingIdentityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == cpfLimpo);
                if (existingIdentityUser != null)
                {
                    ModelState.AddModelError(nameof(Input.Cpf), "Já existe um usuário cadastrado com este CPF no sistema de autenticação.");
                    return Page();
                }

                // Verificar se já existe usuário com esse CPF no sistema legado
                var existingLegacyUser = _usuarioService.GetByCpf(cpfLimpo);
                if (existingLegacyUser != null)
                {
                    ModelState.AddModelError(nameof(Input.Cpf), "Já existe um usuário cadastrado com este CPF no sistema.");
                    return Page();
                }

                // Verificar se já existe usuário com esse email no Identity
                var existingEmailUser = await _userManager.FindByEmailAsync(Input.Email);
                if (existingEmailUser != null)
                {
                    ModelState.AddModelError(nameof(Input.Email), "Já existe um usuário cadastrado com este email.");
                    return Page();
                }

                try
                {
                    // 1. Criar usuário no sistema legado primeiro
                    var usuarioViewModel = new UsuarioViewModel
                    {
                        UsuarioModel = new UsuarioModel
                        {
                            Cpf = cpfLimpo,
                            Nome = Input.FullName,
                            DataNascimento = Input.BirthDate,
                            Senha = Criptography.GeneratePasswordHash(Input.Password)
                        },
                        TipoUsuarioModel = new TipoUsuarioModel
                        {
                            Id = 4 // ID do usuarioPendente 
                        },
                        OrganizacaoModel = _organizacaoService.GetById(Input.OrganizacaoId)
                    };

                    // Inserir no sistema legado
                    var usuarioLegado = _usuarioService.Insert(usuarioViewModel);

                    // 2. Criar usuário no Identity 
                    var identityUser = new ApplicationUser
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        EmailConfirmed = false, 
                        Cpf = cpfLimpo,
                        BirthDate = Input.BirthDate
                    };

                    var result = await _userManager.CreateAsync(identityUser, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Usuário criou uma nova conta com senha.");

                        // 3. Adicionar role padrão de pendente
                        await _userManager.AddToRoleAsync(identityUser, TipoUsuarioModel.ROLE_PENDENTE);

                        // 4. Gerar token de confirmação de email
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                        // 5. Criar link de confirmação
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "", userId = identityUser.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        // 6. Enviar email de confirmação
                        await SendConfirmationEmailAsync(Input.Email, Input.FullName, callbackUrl);

                        // 7. Redirecionar para página de confirmação enviada
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        // Se falhou ao criar no Identity, tentar remover do sistema legado
                        try
                        {
                            _usuarioService.Remove((int)usuarioLegado.UsuarioModel.Id);
                        }
                        catch (Exception cleanupEx)
                        {
                            _logger.LogError(cleanupEx, "Erro ao limpar usuário do sistema legado após falha no Identity");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar usuário");
                    ModelState.AddModelError(string.Empty, "Erro ao criar usuário. Tente novamente.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task SendConfirmationEmailAsync(string email, string fullName, string callbackUrl)
        {
            var emailSubject = "Confirme sua conta - Smart Salas";
            var emailBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <img src=""https://smartsala.itatechjr.com.br/wp-content/uploads/2025/03/SmartSalas-2-WEBP.webp"" 
                         alt=""Smart Salas"" 
                         style=""max-width: 200px; height: auto; display: block; margin: 0 auto;"" />

                        <h3 style='color: #333;'>Bem-vindo!</h3>
                        
                        <p>Olá <strong>{fullName}</strong>,</p>
                        
                        <p>Obrigado por se registrar no sistema Smart Salas. Para ativar sua conta, você precisa confirmar seu endereço de email.</p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{callbackUrl}' 
                               style='background-color: #28a745; color: white; padding: 12px 30px; 
                                      text-decoration: none; border-radius: 5px; display: inline-block;'>
                                Confirmar Email
                            </a>
                        </div>
                        
                        <p><strong>Importante:</strong></p>
                        <ul>
                            <li>Este link expira em 24 horas por motivos de segurança</li>
                            <li>Você só poderá fazer login após confirmar seu email</li>
                            <li>Se você não se registrou, ignore este email</li>
                        </ul>
                        
                        <p>Após confirmar seu email, você poderá fazer login no sistema com:</p>
                        <ul>
                            <li><strong>Email:</strong> {email}</li>
                            <li><strong>Senha:</strong> A senha que você criou durante o registro</li>
                        </ul>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; text-align: center; 
                                font-size: 12px; color: #666;'>
                        <p>Este é um email automático, não responda.</p>
                        <p>© Smart Salas - Sistema de Gerenciamento de Salas</p>
                    </div>
                </div>";

            await _emailSender.SendEmailAsync(email, emailSubject, emailBody);
        }

        private void LoadOrganizacoes()
        {
            var organizacoes = _organizacaoService.GetAll();
            Organizacoes = new SelectList(organizacoes, "Id", "RazaoSocial");
        }
    }
}