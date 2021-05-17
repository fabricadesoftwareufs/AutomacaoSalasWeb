using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanejamentoController : ControllerBase
    {
        private readonly IPlanejamentoService _service;
        public PlanejamentoController(IPlanejamentoService service)
        {
            _service = service;
        }
        // GET: api/Hardware
        [HttpGet]
        public ActionResult Get()
        {
            var planejamentos = _service.GetAll();
            if (planejamentos.Count == 0)
                return NoContent();

            return Ok(planejamentos);
        }

        // GET: api/Hardware/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var planejamento = _service.GetById(id);
            if (planejamento == null)
                return NoContent();

            return Ok(planejamento);
        }

        // POST: api/Hardware
        [HttpPost]
        public ActionResult Post([FromBody] PlanejamentoModel planejamentoModel) => _service.Insert(planejamentoModel) ? Ok(true) : Ok(false);

        // PUT: api/Hardware/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] PlanejamentoModel planejamentoModel) => _service.Update(planejamentoModel) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id, bool excluirReservas) => _service.Remove(id, excluirReservas) ? Ok(true) : Ok(false);
    }
}
