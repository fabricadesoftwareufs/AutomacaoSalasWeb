using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Http;
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
        public ActionResult Get() => Ok(_service.GetAll());

        // GET: api/Bloco/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var bloco = _service.GetById(id);
            if (bloco == null)
                return NotFound();

            return Ok(bloco);
        }

        // POST: api/Bloco
        [HttpPost]
        public ActionResult Post([FromBody] BlocoModel blocoModel)
        {
            if (_service.Insert(blocoModel))
                return Ok();

            return BadRequest();
        }

        // PUT: api/Bloco/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] BlocoModel blocoModel)
        {
            if (_service.Update(blocoModel))
                return Ok();

            return BadRequest();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (_service.Remove(id))
                return Ok();

            return BadRequest();
        }
    }
}
