using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaController : ControllerBase
    {
        private readonly ISalaService _service;
        public SalaController(ISalaService service)
        {
            _service = service;
        }
        // GET: api/Sala
        [HttpGet]
        public ActionResult Get()
        {
            var salas = _service.GetAll();
            if (salas.Count == 0)
                return NoContent();

            return Ok(salas);
        }

        // GET: api/Sala/5
        [HttpGet("{id}")]

        public ActionResult Get(int id)
        {
            var sala = _service.GetById(id);
            if (sala == null)
                return NoContent();

            return Ok(sala);
        }

        // POST: api/Sala
        [HttpPost]
        public ActionResult Post([FromBody] SalaModel salaModel) => _service.Insert(salaModel) != null ? Ok(true) : Ok(false);

        // PUT: api/Sala/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] SalaModel salaModel) => _service.Update(salaModel) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
