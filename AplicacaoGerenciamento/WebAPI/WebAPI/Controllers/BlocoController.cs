using Business;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocoController : ControllerBase
    {
        private readonly BlocoService _service;
        public BlocoController(BlocoService service)
        {
            _service = service;
        }
        // GET: api/Bloco
        [HttpGet]
        public ActionResult Get()
        {
            var blocos = _service.GetAll();
            if (blocos.Count == 0)
                return Ok();

            return NoContent();
        }

        // GET: api/Bloco/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var bloco = _service.GetById(id);
            if (bloco == null)
                return NoContent();

            return Ok(bloco);
        }

        // POST: api/Bloco
        [HttpPost]
        public ActionResult Post([FromBody] BlocoModel blocoModel) => _service.Insert(blocoModel) ? Ok(true) : Ok(false);

        // PUT: api/Bloco/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] BlocoModel blocoModel) => _service.Update(blocoModel) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
