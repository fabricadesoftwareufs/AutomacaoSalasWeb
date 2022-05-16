using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioSalaController : ControllerBase
    {
        private readonly IHorarioSalaService _service;
        private readonly IHardwareDeSalaService _hardwareService;

        public HorarioSalaController(IHorarioSalaService service)
        {
            _service = service;
        }
        // GET: api/HorarioSala
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var horarios = _service.GetAll();
                if (horarios.Count == 0)
                    return NoContent();

                return Ok(horarios);

            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/HorarioSala/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var horario = _service.GetById(id);
                if (horario == null)
                    return NotFound("Reserva não encontrada na base de dados");

                return Ok(horario);

            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDaSala/{idSala}")]
        public ActionResult GetReservasDaSala(int idSala)
        {
            try
            {
                var horarios = _service.GetByIdSala(idSala);
                if (horarios.Count == 0)
                    return NoContent();

                return Ok(horarios);

            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDaSemana/{idSala}")]
        public ActionResult GetReservasDaSamana(int idSala)
        {
            try
            {
                var horarios = _service.GetReservasDaSemanaByIdSala(idSala);
                if (horarios.Count == 0)
                    return NoContent();

                return Ok(horarios);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDeHoje/{idSala}")]
        public ActionResult GetReservasDeHoje(int idSala)
        {
            try
            {
                var horarios = _service.GetReservasDeHojeByIdSala(idSala);
                
                if (horarios.Count == 0)
                    return StatusCode((int)HttpStatusCode.NoContent, new 
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhuma reserva encontrado!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Horários obtidos com sucesso!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = "[ERROR]: " + e.Message
                });
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDeHojePorUuid/{uuid}")]
        public ActionResult GetReservasDeHojeByUuid(string uuid)
        {
            try
            {
                var horarios = _service.GetReservasDeHojeByUuid(uuid);

                if (horarios.Count == 0)
                    return StatusCode((int)HttpStatusCode.NoContent, new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhuma reserva encontrado!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Horários obtidos com sucesso!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = "[ERROR]: " + e.Message
                });
            }
        }

        // POST: api/HorarioSala
        [HttpPost]
        public ActionResult Post([FromBody] HorarioSalaModel horarioSala)
        {

            try
            {
                if (_service.Insert(horarioSala))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }


        }


        // PUT: api/HorarioSala/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] HorarioSalaModel horarioSala)
        {
            try
            {
                if (_service.Update(horarioSala))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
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
                return StatusCode(500, e.Message);
            }
        }
    }
}
