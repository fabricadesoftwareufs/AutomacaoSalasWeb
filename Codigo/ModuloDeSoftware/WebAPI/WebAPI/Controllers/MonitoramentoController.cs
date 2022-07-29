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
    [AllowAnonymous]
    public class MonitoramentoController : ControllerBase
    {

        private readonly IMonitoramentoService _service;
        public MonitoramentoController(IMonitoramentoService service)
        {
            _service = service;
        }

        // GET: api/Monitoramento/5
        [HttpGet]
        [Route("ObterPorSala/{idSala}")]
        public ActionResult Get(int idEquipamento)
        {
            try
            {
                var monitoramento = _service.GetByIdEquipamento(idEquipamento);
                if (monitoramento == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Monitoramento encontrado!"
                    });

                return Ok(new {
                        result = monitoramento,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Monitoramento retornado com sucesso!"
                    });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }

        }

        // GET: api/Monitoramento/5
        [HttpGet]
        [Route("obter-por-sala-tipo-equipamento/{idSala}/{tipoEquipamento}")]
        public ActionResult GetMonitoramentoByIdSala([FromRoute] int idSala, [FromRoute] string tipoEquipamento)
        {
            try
            {
                var monitoramento = _service.GetByIdSalaAndTipoEquipamento(idSala, tipoEquipamento);

                if (monitoramento == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Monitoramento encontrado!"
                    });

                return Ok(new
                {
                    result = monitoramento,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Monitoramento retornado com sucesso!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }

        }

        // PUT: api/Monitoramento/5
        [HttpPut]
        public ActionResult Atualizar([FromBody] MonitoramentoModel monitoramento)
        {
            try
            {
                if (_service.Update(monitoramento))
                    return Ok(new {
                        result = monitoramento,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Monitoramento atualizado!"
                    }); 

                return BadRequest(new
                {
                    result = monitoramento,
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Houve um problema ao atualizar monitoramento!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
