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
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class UsuarioOrganizacaoController : ControllerBase
    {
        private readonly IUsuarioOrganizacaoService _service;
        public UsuarioOrganizacaoController(IUsuarioOrganizacaoService service)
        {
            _service = service;
        }

        // GET: api/UsuarioOrganizacao
        [HttpGet]
        [AllowAnonymous]
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
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
          
        }

        // GET: api/UsuarioOrganizacao/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult Get(uint id)
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
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
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
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
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
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(uint id)
        {
            try
            {
                if (_service.Remove(id))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
