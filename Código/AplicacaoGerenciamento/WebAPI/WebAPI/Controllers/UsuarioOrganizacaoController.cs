using Service;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioOrganizacaoController : ControllerBase
    {
        private readonly UsuarioOrganizacoesService _service;
        public UsuarioOrganizacaoController(UsuarioOrganizacoesService service)
        {
            _service = service;
        }
        // GET: api/UsuarioOrganizacao
        [HttpGet]
        public ActionResult Get()
        {
            var usuariosOrganizacao = _service.GetAll();
            if (usuariosOrganizacao.Count == 0)
                return NoContent();

            return Ok(usuariosOrganizacao);
        }

        // GET: api/UsuarioOrganizacao/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var usuarioOrganizacao = _service.GetById(id);
            if (usuarioOrganizacao == null)
                return NoContent();

            return Ok(usuarioOrganizacao);
        }

        // POST: api/UsuarioOrganizacao
        [HttpPost]
        public ActionResult Post([FromBody] UsuarioOrganizacaoModel usuarioOrganizacao) => _service.Insert(usuarioOrganizacao) ? Ok(true) : Ok(false);

        // PUT: api/UsuarioOrganizacao/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] UsuarioOrganizacaoModel usuarioOrganizacao) => _service.Update(usuarioOrganizacao) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
