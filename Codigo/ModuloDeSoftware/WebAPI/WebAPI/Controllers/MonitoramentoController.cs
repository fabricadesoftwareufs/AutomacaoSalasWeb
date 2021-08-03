using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoramentoController : ControllerBase
    {

        private readonly IMonitoramentoService _service;
        public MonitoramentoController(IMonitoramentoService service)
        {
            _service = service;
        }

        // GET: api/Monitoramento/5
        [HttpGet("{idSala}")]
        [Route("ObterPorSala")]
        public ActionResult Get(int idSala)
        {
            try
            {
                var monitoramento = _service.GetByIdSala(idSala);
                if (monitoramento == null)
                    return StatusCode(204, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum sala encontrada!"
                    });

                return Ok(new {
                        result = monitoramento,
                        httpCode = 200,
                        message = "Nenhum sala encontrada!"
                    });
            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = 500,
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
                        httpCode = 204,
                        message = "Nenhum sala encontrada!"
                    }); );

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
