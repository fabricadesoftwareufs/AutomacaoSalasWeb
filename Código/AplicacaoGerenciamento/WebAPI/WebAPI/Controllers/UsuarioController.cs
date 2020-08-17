using Service;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }
        // GET: api/Usuario
        [HttpGet]
        public ActionResult Get()
        {
            var usuarios = _service.GetAll();
            if (usuarios.Count == 0)
                return NoContent();

            return Ok(usuarios);
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var usuario = _service.GetById(id);
            if (usuario == null)
                return NoContent();

            return Ok(usuario);
        }

        // POST: api/Usuario
        [HttpPost]
        public ActionResult Post([FromBody] UsuarioModel usuario) => _service.Insert(usuario) ? Ok(true) : Ok(false);

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] UsuarioModel usuario) => _service.Update(usuario) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
