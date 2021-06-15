using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioOrganizacaoController : ControllerBase
    {
        private readonly IUsuarioOrganizacaoService _service;
        public UsuarioOrganizacaoController(IUsuarioOrganizacaoService service)
        {
            _service = service;
        }

        // GET: api/UsuarioOrganizacao
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var usuariosOrganizacao = _service.GetAll();
                if (usuariosOrganizacao.Count == 0)
                    return NoContent();

                return Ok(usuariosOrganizacao);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
          
        }

        // GET: api/UsuarioOrganizacao/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var usuarioOrganizacao = _service.GetById(id);
                if (usuarioOrganizacao == null)
                    return NoContent();

                return Ok(usuarioOrganizacao);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
          
        }

        // POST: api/UsuarioOrganizacao
        [HttpPost]
        public ActionResult Post([FromBody] UsuarioOrganizacaoModel usuarioOrganizacao)
        {

            try
            {
                if (_service.Insert(usuarioOrganizacao))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: api/UsuarioOrganizacao/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] UsuarioOrganizacaoModel usuarioOrganizacao)
        {
            try
            {
                if (_service.Update(usuarioOrganizacao))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_service.Remove(id))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
