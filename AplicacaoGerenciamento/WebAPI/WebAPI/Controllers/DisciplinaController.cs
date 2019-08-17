using Business;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinaController : ControllerBase
    {
        private readonly DisciplinaService _service;
        public DisciplinaController(DisciplinaService service)
        {
            _service = service;
        }
        // GET: api/Disciplina
        [HttpGet]
        public ActionResult Get()
        {
            var disciplinas = _service.GetAll();
            if (disciplinas.Count == 0)
                return NoContent();

            return Ok(disciplinas);
        }

        // GET: api/Disciplina/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var disciplina = _service.GetById(id);
            if (disciplina == null)
                return NoContent();

            return Ok(disciplina);
        }

        // POST: api/Disciplina
        [HttpPost]
        public ActionResult Post([FromBody] DisciplinaModel disciplina) => _service.Insert(disciplina) ? Ok(true) : Ok(false);

        // PUT: api/Disciplina/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] DisciplinaModel disciplina) => _service.Update(disciplina) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
