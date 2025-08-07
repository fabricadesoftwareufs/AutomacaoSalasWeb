using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.ViewModel;
using SalasWeb.Data;
using SalasWeb.ViewModels;
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

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IOrganizacaoService organizacaoService,
            IUsuarioService usuarioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _organizacaoService = organizacaoService;
            _usuarioService = usuarioService;
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
                            Id = 3 // ID do COLABORADOR - ajuste conforme sua base de dados
                        },
                        OrganizacaoModel = _organizacaoService.GetById(Input.OrganizacaoId)
                    };

                    // Inserir no sistema legado
                    var usuarioLegado = _usuarioService.Insert(usuarioViewModel);

                    // 2. Criar usuário no Identity - USAR EMAIL COMO USERNAME
                    var identityUser = new ApplicationUser
                    {
                        UserName = Input.Email, // ✅ CORREÇÃO: Usar email como username
                        Email = Input.Email,
                        EmailConfirmed = true, // Marcar como confirmado
                        Cpf = cpfLimpo,
                        BirthDate = Input.BirthDate
                    };

                    var result = await _userManager.CreateAsync(identityUser, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Usuário criou uma nova conta com senha.");

                        // 3. Adicionar role padrão de colaborador
                        await _userManager.AddToRoleAsync(identityUser, TipoUsuarioModel.ROLE_COLABORADOR);

                        // 4. Adicionar claims personalizados para integração com sistema legado
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, usuarioLegado.UsuarioModel.Id.ToString()), // ID do sistema legado
                            new Claim(ClaimTypes.UserData, usuarioLegado.UsuarioModel.Cpf), // CPF
                            new Claim(ClaimTypes.Name, usuarioLegado.UsuarioModel.Nome) // Nome
                        };

                        await _userManager.AddClaimsAsync(identityUser, claims);

                        TempData["SuccessMessage"] = "Conta criada com sucesso! Você já pode fazer login com seu email e senha.";
                        return RedirectToPage("./Login");
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

        private void LoadOrganizacoes()
        {
            var organizacoes = _organizacaoService.GetAll();
            Organizacoes = new SelectList(organizacoes, "Id", "RazaoSocial");
        }
    }
}