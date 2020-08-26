using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HardwareDeBlocoController : ControllerBase
    {
        private readonly IHardwareDeBlocoService _service;
        public HardwareDeBlocoController(IHardwareDeBlocoService service)
        {
            _service = service;
        }
        // GET: api/Hardware
        [HttpGet]
        public ActionResult Get()
        {
            var hardwares = _service.GetAll();
            if (hardwares.Count == 0)
                return NoContent();

            return Ok(hardwares);
        }

        // GET: api/Hardware/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var hardware = _service.GetById(id);
            if (hardware == null)
                return NoContent();

            return Ok(hardware);
        }

        // POST: api/Hardware
        [HttpPost]
        public ActionResult Post([FromBody] HardwareDeBlocoModel hardwareModel, int idUser) => _service.Insert(hardwareModel, idUser) ? Ok(true) : Ok(false);

        // PUT: api/Hardware/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] HardwareDeBlocoModel hardwareModel, int idUser) => _service.Update(hardwareModel, idUser) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
