using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;

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
            try
            {
                var salas = _service.GetAll();
                if (salas.Count == 0)
                    return NoContent();

                return Ok(salas);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        // GET: api/Sala/5
        [HttpGet("{id}")]

        public ActionResult Get(int id)
        {
            try
            {
                var sala = _service.GetById(id);
                if (sala == null)
                    return StatusCode((int)HttpStatusCode.OK, new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Sala não encontrada na base de dados."
                    });

                return Ok(sala);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        // POST: api/Sala
        [HttpPost]
        public ActionResult Post([FromBody] SalaModel salaModel)
        {
            try
            {
                var sala = _service.Insert(salaModel);
                if (sala != null)
                    return Ok(sala.Id);

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // PUT: api/Sala/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] SalaModel salaModel)
        {
            try
            {
                if (_service.Update(salaModel))
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
