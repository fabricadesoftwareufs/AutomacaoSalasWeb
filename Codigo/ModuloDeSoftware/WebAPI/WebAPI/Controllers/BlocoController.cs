using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocoController : ControllerBase
    {
        private readonly IBlocoService _service;
        public BlocoController(IBlocoService service)
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
                return NotFound("Bloco não encontrado na base de dados");

            return Ok(bloco);
        }

        // POST: api/Bloco
        [HttpPost]
        public ActionResult Post([FromBody] BlocoModel blocoModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _service.Insert(blocoModel);
                    if (resultado != null)
                        return Ok(resultado.Id);
                }
            }
            catch(ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Bloco/5
        [HttpPut]
        public ActionResult Put([FromBody] BlocoModel blocoModel)
        {
            try
            {
                if (ModelState.IsValid && _service.Update(blocoModel))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_service.Remove(id))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest();
        }
    }
}
