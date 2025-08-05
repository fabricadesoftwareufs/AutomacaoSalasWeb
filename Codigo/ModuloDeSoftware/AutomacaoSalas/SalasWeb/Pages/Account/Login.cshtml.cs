using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalasWeb.Data;
using SalasWeb.ViewModels;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == Methods.CleanString(Input.Cpf));
                
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    
                    if (result.Succeeded)
                    {
                        // Buscar dados do usuário no sistema legado
                        var usuarioLegado = _usuarioService.GetByCpf(Methods.CleanString(Input.Cpf));
                        
                        if (usuarioLegado != null)
                        {
                            // Verificar se as claims já existem
                            var existingClaims = await _userManager.GetClaimsAsync(user);
                            
                            // Adicionar claims se não existirem
                            var claimsToAdd = new List<Claim>();
                            
                            if (!existingClaims.Any(c => c.Type == ClaimTypes.SerialNumber))
                            {
                                claimsToAdd.Add(new Claim(ClaimTypes.SerialNumber, usuarioLegado.Id.ToString()));
                            }
                            
                            if (!existingClaims.Any(c => c.Type == ClaimTypes.UserData))
                            {
                                claimsToAdd.Add(new Claim(ClaimTypes.UserData, usuarioLegado.Cpf));
                            }
                            
                            if (!existingClaims.Any(c => c.Type == ClaimTypes.NameIdentifier))
                            {
                                claimsToAdd.Add(new Claim(ClaimTypes.NameIdentifier, usuarioLegado.Nome));
                            }

                            // Buscar tipo de usuário
                            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(usuarioLegado.Id);
                            if (tipoUsuario != null && !existingClaims.Any(c => c.Type == ClaimTypes.Role && c.Value == tipoUsuario.Descricao))
                            {
                                claimsToAdd.Add(new Claim(ClaimTypes.Role, tipoUsuario.Descricao));
                            }

                            if (claimsToAdd.Any())
                            {
                                await _userManager.AddClaimsAsync(user, claimsToAdd);
                            }
                        }
                        
                        await _signInManager.RefreshSignInAsync(user);
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