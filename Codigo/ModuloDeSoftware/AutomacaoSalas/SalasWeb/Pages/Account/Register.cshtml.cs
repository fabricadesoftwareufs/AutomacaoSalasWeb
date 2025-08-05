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
        private readonly IUsuarioService _usuarioService; // Adicionar o serviço legado

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IOrganizacaoService organizacaoService,
            IUsuarioService usuarioService) // Adicionar ao construtor
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

                // Verificar se já existe usuário com esse CPF no Identity
                var existingIdentityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == Methods.CleanString(Input.Cpf));
                if (existingIdentityUser != null)
                {
                    ModelState.AddModelError(nameof(Input.Cpf), "Já existe um usuário cadastrado com este CPF no sistema de autenticação.");
                    return Page();
                }

                // Verificar se já existe usuário com esse CPF no sistema legado
                var existingLegacyUser = _usuarioService.GetByCpf(Methods.CleanString(Input.Cpf));
                if (existingLegacyUser != null)
                {
                    ModelState.AddModelError(nameof(Input.Cpf), "Já existe um usuário cadastrado com este CPF no sistema.");
                    return Page();
                }

                // 1. Criar usuário no sistema legado primeiro
                var usuarioViewModel = new UsuarioViewModel
                {
                    UsuarioModel = new UsuarioModel
                    {
                        Cpf = Methods.CleanString(Input.Cpf),
                        Nome = Input.FullName,
                        DataNascimento = Input.BirthDate,
                        Senha = Criptography.GeneratePasswordHash(Input.Password) // Usar a mesma criptografia do sistema legado
                    },
                    TipoUsuarioModel = new TipoUsuarioModel
                    {
                        Id = 3 // ID do COLABORADOR - ajuste conforme sua base de dados
                    },
                    OrganizacaoModel = _organizacaoService.GetById(Input.OrganizacaoId)
                };

                try
                {
                    // Inserir no sistema legado
                    var usuarioLegado = _usuarioService.Insert(usuarioViewModel);

                    // 2. Criar usuário no Identity
                    var identityUser = new ApplicationUser
                    {
                        UserName = Methods.CleanString(Input.Cpf),
                        Email = Input.Email,
                        Cpf = Methods.CleanString(Input.Cpf),
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
                            new Claim(ClaimTypes.SerialNumber, usuarioLegado.UsuarioModel.Id.ToString()),
                            new Claim(ClaimTypes.UserData, usuarioLegado.UsuarioModel.Cpf),
                            new Claim(ClaimTypes.NameIdentifier, usuarioLegado.UsuarioModel.Nome)
                        };

                        await _userManager.AddClaimsAsync(identityUser, claims);

                        TempData["SuccessMessage"] = "Conta criada com sucesso! Você já pode fazer login.";
                        return RedirectToPage("./Login");
                    }
                    else
                    {
                        // Se falhou ao criar no Identity, remover do sistema legado
                        _usuarioService.Remove((int)usuarioLegado.UsuarioModel.Id);

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