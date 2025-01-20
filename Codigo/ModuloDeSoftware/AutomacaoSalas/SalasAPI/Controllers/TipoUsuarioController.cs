using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly ITipoUsuarioService _service;
        public TipoUsuarioController(ITipoUsuarioService service)
        {
            _service = service;
        }
        // GET: api/TipoUsuario
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var tipoUser = _service.GetAll();
                if (tipoUser.Count == 0)
                    return NoContent();

                return Ok(tipoUser);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
           
        }


        /*
        // TODO: M4RCOSVS0 - Revisar e corrigir o método public ActionResult. Há problemas que precisam ser ajustados, mas não serão feitos agora.
        // GET: api/TipoUsuario/5
        [HttpGet("{id}")]
        public ActionResult Get(uint id)
        {
            try
            {
                var tipo = _service.GetById(id);
                if (tipo == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        mesage = "Tipo de Usuário não encontrado na base de dados."
                    });

                return Ok(tipo);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
           
        }
        */
    }
}
