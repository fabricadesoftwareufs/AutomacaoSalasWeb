using Service;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoHardwareController : ControllerBase
    {
        private readonly TipoHardwareService _service;
        public TipoHardwareController(TipoHardwareService service)
        {
            _service = service;
        }

        // GET api/TipoHardware
        [HttpGet]
        public ActionResult Get()
        {
            var tipoHard = _service.GetAll();
            if (tipoHard.Count == 0)
                return NoContent();

            return Ok(tipoHard);
        }

        // GET api/TipoHardware/6
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var tipo = _service.GetById(id);
            if (tipo == null)
                return NoContent();

            return Ok(tipo);
        }

        // POST api/TipoHardware
        [HttpPost]
        public ActionResult Post([FromBody] TipoHardwareModel tipoHardware) => _service.Insert(tipoHardware) ? Ok(true) : Ok(false);

        // PUT api/TipoHardware/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] TipoHardwareModel tipoHardware) => _service.Update(tipoHardware) ? Ok(true) : Ok(false);

        // DELETE api/TipoHardware/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
