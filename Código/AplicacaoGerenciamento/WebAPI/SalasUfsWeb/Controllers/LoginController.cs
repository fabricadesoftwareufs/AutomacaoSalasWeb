using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;
using Utils;

namespace SalasUfsWeb.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUsuarioService _service;
        public LoginController(IUsuarioService service)
        {
            _service = service;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> Authenticate(LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {
                if (ValidaCpf(loginViewModel.Login))
                {
                    // Obtendo o usuario baseado nas informações passadas.
                    var user = _service.GetByLoginAndPass(Methods.CleanString(loginViewModel.Login), Criptography.GeneratePasswordHash(loginViewModel.Senha));
                    if (user != null)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                        new Claim(ClaimTypes.UserData, user.Cpf),
                        new Claim(ClaimTypes.NameIdentifier, user.Nome),
                        new Claim(ClaimTypes.DateOfBirth, user.DataNascimento.ToString()),
                        new Claim(ClaimTypes.Role, user.TipoUsuarioId.ToString())
                    };

                        // Adicionando uma identidade as claims.
                        var identity = new ClaimsIdentity(claims, "login");

                        // Propriedades da autenticação.
                        var claimProperty = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1) // Expira em 1 dia
                        };

                        // Logando efetivamente.
                        await HttpContext.SignInAsync(new ClaimsPrincipal(identity), claimProperty);

                        // Redirecionando, com usuario logado.
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // usuario invalido.
            return RedirectToAction("Index", "Login", new { msg = "error" });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        [Authorize]
        public ActionResult AcessoNegado() => View();
        public bool ValidaCpf(string cpf) => Methods.ValidarCpf(cpf);
    }
}