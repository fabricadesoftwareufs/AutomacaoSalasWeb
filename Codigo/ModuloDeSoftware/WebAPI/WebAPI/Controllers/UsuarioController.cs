using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IOrganizacaoService _organizacaoService;

        public UsuarioController(IUsuarioService service, IOrganizacaoService organizacaoService)
        {
            _service = service;
            _organizacaoService = organizacaoService;

        }

        // GET: api/Usuario
        [HttpGet]
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
                return StatusCode(500, e.Message);
            }
           
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
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
                return StatusCode(500, e.Message);
            }
           
        }
    }
}
