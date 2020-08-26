using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioSalaController : ControllerBase
    {
        private readonly IHorarioSalaService _service;
        public HorarioSalaController(IHorarioSalaService service)
        {
            _service = service;
        }
        // GET: api/HorarioSala
        [HttpGet]
        public ActionResult Get()
        {
            var horarios = _service.GetAll();
            if (horarios.Count == 0)
                return NoContent();

            return Ok(horarios);
        }

        // GET: api/HorarioSala/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var horario = _service.GetById(id);
            if (horario == null)
                return NoContent();

            return Ok(horario);
        }

        // POST: api/HorarioSala
        [HttpPost]
        public ActionResult Post([FromBody] HorarioSalaModel horarioSala) => _service.Insert(horarioSala) ? Ok(true) : Ok(false);

        // PUT: api/HorarioSala/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] HorarioSalaModel horarioSala) => _service.Update(horarioSala) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
