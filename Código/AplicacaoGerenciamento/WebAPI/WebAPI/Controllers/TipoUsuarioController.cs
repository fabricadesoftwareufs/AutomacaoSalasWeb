using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var tipoUser = _service.GetAll();
            if (tipoUser.Count == 0)
                return NoContent();

            return Ok(tipoUser);
        }

        // GET: api/TipoUsuario/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var tipo = _service.GetById(id);
            if (tipo == null)
                return NoContent();

            return Ok(tipo);
        }

        // POST: api/TipoUsuario
        [HttpPost]
        public ActionResult Post([FromBody] TipoUsuarioModel tipoUsuario) => _service.Insert(tipoUsuario) ? Ok(true) : Ok(false);

        // PUT: api/TipoUsuario/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] TipoUsuarioModel tipoUsuario) => _service.Update(tipoUsuario) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
