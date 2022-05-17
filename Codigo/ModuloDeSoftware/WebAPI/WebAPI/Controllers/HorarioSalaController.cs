using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;
using Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioSalaController : ControllerBase
    {
        private readonly IHorarioSalaService _service;
        private readonly IHardwareDeSalaService _hardwareService;
        public HorarioSalaController(IHorarioSalaService service, IHardwareDeSalaService hardwareService)
        {
            _service = service;
            _hardwareService = hardwareService;
        }
        // GET: api/HorarioSala
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var horarios = _service.GetAll();
                if (horarios.Count == 0)
                    return StatusCode(204, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum Horário encontrado!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = 200,
                    message = "Horário encontrado com sucesso!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = 500,
                    message = e.Message
                });
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
                    return StatusCode(204, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum Horário encontrado!"
                    });

                return Ok(new
                {
                    result = horario,
                    httpCode = 200,
                    message = "Horário encontrado com sucesso!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = 500,
                    message = e.Message
                });
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
                    return StatusCode(204, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum Horário encontrado!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = 200,
                    message = "Horário retornado com sucesso!"
                });

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
                    return StatusCode(204, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum Reserva encontrada!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = 200,
                    message = "Reservas retornadas com sucesso!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = 500,
                    message = e.Message
                });
            }
        }


        // GET: api/Hardware/5
        [HttpGet("{uuid}/get-horarios-sala")]
        public ActionResult GetHorarioSemana([FromRoute] string uuid, [FromQuery] string token)
        {

            try
            {
                var hardware = _hardwareService.GetByUuid(uuid);
                if (hardware == null)
                    return NotFound(new
                    {
                        result = "null",
                        httpCode = 404,
                        message = "Hardware não foi encontrado na base de dados com esse uuid",
                    });

                else if (string.IsNullOrEmpty(token) && !token.Equals(hardware.Token) && (string.IsNullOrEmpty(hardware.Token)))
                    return StatusCode((int)HttpStatusCode.Unauthorized, new
                    {
                        result = "null",
                        httpCode = 401,
                        message = "O token é inválido!"
                    });

                else if (hardware.Uuid == null)
                    return StatusCode((int)HttpStatusCode.Unauthorized, new
                    {
                        result = "null",
                        httpCode = 401,
                        message = "Erro crasso, Hardware não está registrado!"
                    });
                else
                {
                    var horarios = _service.GetReservasDaSemanaByIdSala(hardware.SalaId);
                    if (horarios.Count > 0)
                        return Ok(new
                        {
                            result = new { schedules = horarios },
                            httpCode = 200,
                            message = "Horarios obtidos com sucesso",
                        });
                    else
                        return NotFound(new
                        {
                            result = "null",
                            httpCode = 400,
                            message = "Horarios não foram encontrados",
                        });
                }
            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = 500,
                    message = e.Message
                });
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDeHoje/{idSala}")]
        public ActionResult GetReservasDeHje(int idSala)
        {
            try
            {
                var horarios = _service.GetReservasDeHojeByIdSala(idSala);
                if (horarios.Count == 0)
                    return NoContent();

                return Ok(horarios);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST: api/HorarioSala
        [HttpPost]
        public ActionResult Post([FromBody] HorarioSalaModel horarioSala)
        {

            try
            {
                if (_service.Insert(horarioSala))
                    return Ok(new
                    {
                        result = horarioSala,
                        httpCode = 200,
                        message = "Horário criado com sucesso!"
                    });

                return BadRequest(new
                {
                    result = "null",
                    httpCode = 400,
                    message = "Houve um problema na inclusão do horário!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = 500,
                    message = e.Message
                });
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

                return BadRequest(new
                {
                    result = "null",
                    httpCode = 200,
                    message = "Houve um problema na atualização do horário!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode(500,
                     new
                     {
                         result = "null",
                         httpCode = 500,
                         message = e.Message
                     });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_service.Remove(id))
                    return Ok(new
                    {
                        result = "null",
                        httpCode = 200,
                        message = "Horário excluido com sucesso!"
                    });

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = 500,
                    message = e.Message
                });
            }
        }
    }
}
