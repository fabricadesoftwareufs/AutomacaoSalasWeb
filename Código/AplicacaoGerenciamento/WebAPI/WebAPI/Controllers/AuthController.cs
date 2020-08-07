using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.ViewModel;
using Service;
using Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UsuarioService _service;

        public AuthController(IConfiguration configuration, UsuarioService service)
        {
            _configuration = configuration;
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] LoginViewModel request)
        {
            var user = _service.GetByLoginAndPass(Methods.CleanString(request.Login), Criptography.GeneratePasswordHash(request.Senha));
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


                // Recebe uma instancia da classe SymmetricSecurityKey e armazena a key de segurança settada no appsettings.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

                // Recebe uma instancia de SigningCredentials contendo a chave de critpgrafia e o algoritmo q fará a encr/descriptogafia
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                // Criando o token efetivamente
                var token = new JwtSecurityToken(
                    issuer: "ProjetoSalasUFS", // Emissor
                    audience: "ProjetoSalasUFS",
                    claims: claims,
                    expires: DateTime.Now.AddDays(1), // Expiração do token
                    signingCredentials: credentials
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Credenciais Invalidas");
        }
    }
}