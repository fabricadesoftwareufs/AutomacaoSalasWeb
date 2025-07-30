using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.ViewModel;
using SalasWeb.Data;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utils;

namespace SalasWeb.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                    var user = await userManager.Users.FirstOrDefaultAsync(u => u.Cpf == Methods.CleanString(loginViewModel.Login));
                    var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Senha, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.UserData, user.Cpf));
                        await signInManager.RefreshSignInAsync(user);
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