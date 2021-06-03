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
        // GET: api/SalaParticular
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var salasParticular = _service.GetAll();
                if (salasParticular.Count == 0)
                    return NoContent();

                return Ok(salasParticular);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }  
        }

        // GET: api/SalaParticular/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var salaParticular = _service.GetById(id);
                if (salaParticular == null)
                    return NotFound("Sala exclusiva não encontrada na base de dados.");

                return Ok(salaParticular);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
           
        }

        // POST: api/SalaParticular
        [HttpPost]
        public ActionResult Post([FromBody] SalaParticularModel salaParticularModel)
        {
            try
            {
                if (ModelState.IsValid && _service.Insert(salaParticularModel))
                        return Ok();  
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/SalaParticular/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SalaParticularModel salaParticularModel)
        {
            try
            {
                if (ModelState.IsValid && _service.Update(salaParticularModel))
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
