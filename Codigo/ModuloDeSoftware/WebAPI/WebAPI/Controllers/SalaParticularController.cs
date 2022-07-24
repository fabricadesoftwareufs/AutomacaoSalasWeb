using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;

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
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
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
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Sala exclusiva não encontrada na base de dados."
                    });

                return Ok(salaParticular);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
           
        }

        // POST: api/SalaParticular
        [HttpPost]
        public ActionResult Post([FromBody] SalaParticularModel salaParticularModel)
        {
            try
            {
                if (_service.Insert(salaParticularModel))
                        return Ok();  

                 return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // PUT: api/SalaParticular/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SalaParticularModel salaParticularModel)
        {
            try
            {
                if (ModelState.IsValid && _service.Update(salaParticularModel))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }        
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_service.Remove(id))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
