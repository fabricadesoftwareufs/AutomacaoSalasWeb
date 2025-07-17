using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModel;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Utils;

namespace SalasWeb.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;//trocar pelo do identity e desenrolar é doido é.
        private readonly ITipoUsuarioService _tipoUsuarioService;

        public LoginController(IUsuarioService service,
                               ITipoUsuarioService tipoUsuarioService)
        {
            _usuarioService = service;
            _tipoUsuarioService = tipoUsuarioService;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> Authenticate(LoginViewModel loginViewModel)
        {
            if (loginViewModel == null)
            {
                throw new ArgumentNullException(nameof(loginViewModel), "LoginViewModel não pode ser nulo");
            }

            if (ModelState.IsValid)
            {
                if (ValidaCpf(loginViewModel.Login))
                {
                    if (_usuarioService == null)
                    {
                        throw new InvalidOperationException("Serviço de usuário não foi inicializado");
                    }

                    var user = _usuarioService.GetByLoginAndPass(Methods.CleanString(loginViewModel.Login), Criptography.GeneratePasswordHash(loginViewModel.Senha));//vai mudar
                    if (user != null)
                    {
                        if (_tipoUsuarioService == null)
                        {
                            throw new InvalidOperationException("Serviço de tipo de usuário não foi inicializado");
                        }

                        // Busca o tipo de usuário usando o novo método
                        var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(user.Id);
                        if (tipoUsuario == null || tipoUsuario.Descricao == null)
                        {
                            throw new InvalidOperationException("Tipo de usuário não encontrado");
                        }

                        var claims = new List<Claim>//troca pela do identity
                        {
                            new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                            new Claim(ClaimTypes.UserData, user.Cpf),
                            new Claim(ClaimTypes.NameIdentifier, user.Nome),
                            new Claim(ClaimTypes.DateOfBirth, user.DataNascimento.ToString()),
                            new Claim(ClaimTypes.Role, tipoUsuario.Descricao)
                        };

                        var identity = new ClaimsIdentity(claims, "login");

                        var claimProperty = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1) // Expira em 1 dia
                        };

                        await HttpContext.SignInAsync(new ClaimsPrincipal(identity), claimProperty);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

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