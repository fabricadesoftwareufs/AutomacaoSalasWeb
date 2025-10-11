using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalasWeb.Data;
using Service.Interface;
using System.Threading.Tasks;

namespace SalasWeb.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsuarioService _usuarioService;

        public ConfirmEmailModel(
            UserManager<ApplicationUser> userManager,
            IUsuarioService usuarioService)
        {
            _userManager = userManager;
            _usuarioService = usuarioService;
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

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                // Adicionar usuário à role PENDENTE (apenas role, sem claims)
                await _userManager.AddToRoleAsync(user, "PENDENTE");

                // Buscar usuário no sistema legado pelo CPF para obter o ID
                var usuarioLegado = _usuarioService.GetByCpf(user.UserName);
                if (usuarioLegado != null)
                {
                    // Armazenar ID do usuário legado na session
                    HttpContext.Session.SetInt32("LegacyUserId", (int)usuarioLegado.Id);
                    HttpContext.Session.SetString("UserCpf", user.UserName);
                    HttpContext.Session.SetString("UserName", usuarioLegado.Nome);
                }

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