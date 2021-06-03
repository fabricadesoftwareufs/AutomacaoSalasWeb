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

        // GET: api/ReservaSala/5
        [HttpGet("{idSala}")]
        [Route("ObterPorSala")]
        public ActionResult Get(int idSala)
        {
            try
            {
                var monitoramento = _service.GetByIdSala(idSala);
                if (monitoramento == null)
                    return NoContent();

                return Ok(monitoramento);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
           
        }

        // PUT: api/HorarioSala/5
        [HttpPut]
        public ActionResult Atualizar([FromBody] MonitoramentoModel monitoramento)
        {

            try
            {
                if (ModelState.IsValid && _service.Update(monitoramento))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(ModelState);
        }
    }
}
