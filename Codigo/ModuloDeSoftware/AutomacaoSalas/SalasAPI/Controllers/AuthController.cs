using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.ViewModel;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utils;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService; 
        private readonly ITipoUsuarioService _tipoUsuarioService;

        public AuthController(IConfiguration configuration, IUsuarioService usuarioService, ITipoUsuarioService tipoUsuarioService)
        {
            _configuration = configuration;
            _usuarioService = usuarioService;
            _tipoUsuarioService = tipoUsuarioService;
        }
        
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] LoginViewModel request)
        {
            var user = _usuarioService.GetByLoginAndPass(Methods.CleanString(request.Login), Criptography.GeneratePasswordHash(request.Senha));
            if (user != null)
            {
                var userType = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(user.Id);
                if (userType == null)
                {
                    return BadRequest( new { message = "Houve um problema na autenticação, por favor tente novamente em alguns minutos!." } );
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                        new Claim(ClaimTypes.UserData, user.Cpf),
                        new Claim(ClaimTypes.NameIdentifier, user.Nome),
                        new Claim(ClaimTypes.DateOfBirth, user.DataNascimento.ToString()),
                        new Claim(ClaimTypes.Role, userType.Descricao)
                };

                // Recebe uma instancia da classe SymmetricSecurityKey e armazena a key de segurança settada no appsettings.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

                // Recebe uma instancia de SigningCredentials contendo a chave de critpgrafia e o algoritmo q fará a encr/descriptogafia
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Criando o token efetivamente
                var token = new JwtSecurityToken(
                    issuer: "SalasUfsWebApi.net", // Emissor
                    audience: "SalasUfsWebApi.net",
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