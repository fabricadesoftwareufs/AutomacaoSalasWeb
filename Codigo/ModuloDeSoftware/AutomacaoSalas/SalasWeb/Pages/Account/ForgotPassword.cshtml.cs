using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalasWeb.Data;
using SalasWeb.Models.ViewModels;
using System.Threading.Tasks;

namespace SalasWeb.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public ForgotPasswordViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Não revelar que o usuário não existe ou não está confirmado
                    // Por segurança, sempre mostrar a mesma mensagem
                    TempData["StatusMessage"] = "Se o email informado estiver cadastrado em nosso sistema, você receberá um link para redefinir sua senha.";
                    return RedirectToPage("./ForgotPassword");
                }

                // Gerar token de reset de senha
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Criar o link de callback
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "", code = code, email = Input.Email },
                    protocol: Request.Scheme);

                // Enviar email
                var emailSubject = "Redefinir Senha - Smart Salas";
                var emailBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                            <h2 style='color: #333;'>Smart Salas</h2>
                        </div>
                        
                        <div style='padding: 30px; background-color: #ffffff;'>
                            <h3 style='color: #333;'>Redefinir Senha</h3>
                            
                            <p>Olá,</p>
                            
                            <p>Você solicitou a redefinição de sua senha no sistema Smart Salas.</p>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <a href='{callbackUrl}' 
                                   style='background-color: #007bff; color: white; padding: 12px 30px; 
                                          text-decoration: none; border-radius: 5px; display: inline-block;'>
                                    Redefinir Senha
                                </a>
                            </div>
                            
                            <p><strong>Importante:</strong></p>
                            <ul>
                                <li>Este link expira em 24 horas por motivos de segurança</li>
                                <li>Se você não solicitou esta redefinição, ignore este email</li>
                                <li>Sua senha atual permanece inalterada até que você clique no link acima</li>
                            </ul>
                        </div>
                        
                        <div style='background-color: #f8f9fa; padding: 15px; text-align: center; 
                                    font-size: 12px; color: #666;'>
                            <p>Este é um email automático, não responda.</p>
                            <p>© Smart Salas - Sistema de Gerenciamento de Salas</p>
                        </div>
                    </div>";

                await _emailSender.SendEmailAsync(Input.Email, emailSubject, emailBody);

                TempData["StatusMessage"] = "Se o email informado estiver cadastrado em nosso sistema, você receberá um link para redefinir sua senha.";
                return RedirectToPage("./ForgotPassword");
            }

            return Page();
        }
    }
}