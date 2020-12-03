using Microsoft.AspNetCore.Mvc;
using Service.Interface;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaSalaController : ControllerBase
    {
        private readonly IHorarioSalaService _service;
        public ReservaSalaController(IHorarioSalaService service)
        {
            _service = service;
        }
        // GET: api/ReservaSala/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var horarios = _service.GetByIdSala(id);
            if (horarios.Count == 0)
                return NoContent();

            return Ok(horarios);
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDaSemana/{id}")]
        public ActionResult GetReservasDaSamana(int id)
        {
            var horarios = _service.GetReservasDaSemanaByIdSala(id);
            if (horarios.Count == 0)
                return NoContent();

            return Ok(horarios);
        }
    }
}
