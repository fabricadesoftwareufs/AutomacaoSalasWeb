using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SalasWeb.Data;
using System.Threading.Tasks;

namespace SalasWeb.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync();
                _logger.LogInformation("Usuário foi desconectado com sucesso.");
            }

            return RedirectToPage("/Account/Login");
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            _logger.LogInformation("Usuário foi desconectado com sucesso.");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }
    }
}