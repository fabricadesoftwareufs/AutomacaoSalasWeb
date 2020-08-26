using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaParticularController : ControllerBase
    {
        private readonly ISalaParticularService _service;
        public SalaParticularController(SalaParticularService service)
        {
            _service = service;
        }
        // GET: api/Hardware
        [HttpGet]
        public ActionResult Get()
        {
            var salasParticular = _service.GetAll();
            if (salasParticular.Count == 0)
                return NoContent();

            return Ok(salasParticular);
        }

        // GET: api/Hardware/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var salaParticular = _service.GetById(id);
            if (salaParticular == null)
                return NoContent();

            return Ok(salaParticular);
        }

        // POST: api/Hardware
        [HttpPost]
        public ActionResult Post([FromBody] SalaParticularModel salaParticularModel) => _service.Insert(salaParticularModel) ? Ok(true) : Ok(false);

        // PUT: api/Hardware/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SalaParticularModel salaParticularModel) => _service.Update(salaParticularModel) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
