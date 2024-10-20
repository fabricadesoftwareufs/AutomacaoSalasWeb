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
    [Authorize(Roles = TipoUsuarioModel.ADMINISTRATIVE_ROLES)]
    public class PlanejamentoController : ControllerBase
    {
        private readonly IPlanejamentoService _service;
        public PlanejamentoController(IPlanejamentoService service)
        {
            _service = service;
        }
        // GET: api/Planejamento
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            try
            {
                var planejamentos = _service.GetAll();
                if (planejamentos.Count == 0)
                    return NoContent();

                return Ok(planejamentos);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: api/Planejamento/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            try
            {
                var planejamento = _service.GetById(id);
                if (planejamento == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Planejamento não encontrado na base de dados"
                    });

                return Ok(planejamento);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // POST: api/Planejamento
        [HttpPost]
        public ActionResult Post([FromBody] PlanejamentoModel planejamentoModel)
        {
            try
            {
                if (_service.Insert(planejamentoModel))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // PUT: api/Planejamento/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] PlanejamentoModel planejamentoModel)
        {
            try
            {
                if (_service.Update(planejamentoModel))
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
        public ActionResult Delete(int id, bool excluirReservas)
        {
            try
            {
                if (_service.Remove(id, excluirReservas))
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