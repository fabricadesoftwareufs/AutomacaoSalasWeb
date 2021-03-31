using Microsoft.AspNetCore.Mvc;
using Model;
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
            var monitoramento = _service.GetByIdSala(idSala);
            if (monitoramento == null)
                return NoContent();

            return Ok(monitoramento);
        }

        // PUT: api/HorarioSala/5
        [HttpPut]
        public ActionResult Atualizar([FromBody] MonitoramentoModel monitoramento) => _service.Update(monitoramento) ? Ok(true) : Ok(false);

        // DELETE api/<MonitoramentoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}
