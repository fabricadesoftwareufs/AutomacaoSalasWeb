using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: api/Usuario
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            try
            {
                var usuarios = _service.GetAll();
                if (usuarios.Count == 0)
                    return NoContent();

                return Ok(usuarios);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
           
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            try
            {
                var usuario = _service.GetById(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado na base de dados.");

                return Ok(usuario);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
           
        }
    }
}
