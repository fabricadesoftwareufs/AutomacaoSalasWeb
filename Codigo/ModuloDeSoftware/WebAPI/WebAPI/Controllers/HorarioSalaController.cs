using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioSalaController : ControllerBase
    {
        private readonly IHorarioSalaService _service;
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
