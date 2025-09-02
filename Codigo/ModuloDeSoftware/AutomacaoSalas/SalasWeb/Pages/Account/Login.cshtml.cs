using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalasWeb.Data;
using SalasWeb.Models.ViewModels;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace SalasWeb.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoUsuarioService _tipoUsuarioService;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUsuarioService usuarioService,
            ITipoUsuarioService tipoUsuarioService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _usuarioService = usuarioService;
            _tipoUsuarioService = tipoUsuarioService;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                if (!Methods.ValidarCpf(Input.Cpf))
                {
                    ModelState.AddModelError(string.Empty, "CPF inválido!");
                    return Page();
                }

                // Buscar usuário pelo CPF no Identity
                var cpfLimpo = Methods.CleanString(Input.Cpf);
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == cpfLimpo);

                if (user != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Você deve confirmar seu email antes de fazer login.");
                        return Page();
                    }

                    var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Obter usuário do sistema legado pelo CPF
                        var usuarioLegado = _usuarioService.GetByCpf(cpfLimpo);
                        if (usuarioLegado != null)
                        {
                            // Armazenar informações do usuário na session
                            HttpContext.Session.SetInt32("LegacyUserId", (int)usuarioLegado.Id);
                            HttpContext.Session.SetString("UserCpf", cpfLimpo);
                            HttpContext.Session.SetString("UserName", usuarioLegado.Nome);

                            // Obter tipo de usuário do sistema legado
                            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(usuarioLegado.Id);
                            if (tipoUsuario != null)
                            {
                                HttpContext.Session.SetString("UserType", tipoUsuario.Descricao);
                            }

                            // Obter roles do usuário no Identity
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles.Any())
                            {
                                HttpContext.Session.SetString("UserRole", roles.First());
                            }
                        }

                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        return RedirectToPage("./Lockout");
                    }
                }

                ModelState.AddModelError(string.Empty, "CPF ou senha inválidos.");
                return Page();
            }

            return Page();
        }
    }
}