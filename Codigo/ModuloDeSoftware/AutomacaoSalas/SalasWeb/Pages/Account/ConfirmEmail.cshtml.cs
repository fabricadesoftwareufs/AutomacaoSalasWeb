using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalasWeb.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SalasWeb.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code, string returnUrl = null)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);//deixa true o confirma o Email

            if (result.Succeeded)
            {
                // Adicionar claims personalizados após confirmação de email
                // Buscar dados do usuário no sistema legado se necessário
                var claims = new[]
                {
                    new Claim(ClaimTypes.UserData, user.Cpf),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                await _userManager.AddClaimsAsync(user, claims);

                StatusMessage = "Obrigado por confirmar seu email. Sua conta foi ativada com sucesso!";
                ViewData["ShowSuccessMessage"] = true;
            }
            else
            {
                StatusMessage = "Erro ao confirmar seu email. O link pode ter expirado ou ser inválido.";
                ViewData["ShowErrorMessage"] = true;
            }

            ViewData["ReturnUrl"] = returnUrl;
            return Page();
        }
    }
}