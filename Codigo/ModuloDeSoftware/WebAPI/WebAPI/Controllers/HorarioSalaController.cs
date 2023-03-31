using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Persistence;
using Service;
using Service.Interface;
using System;
using System.Net;
using System.Security.Claims;
using Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
    public class HorarioSalaController : ControllerBase
    {
        private readonly IHorarioSalaService _service;
        private readonly IHardwareDeSalaService _hardwareService;
        private readonly ISalaService _salaService;
        private readonly IBlocoService _blocoService;
        private readonly IMonitoramentoService _monitoramentoService;
        public HorarioSalaController(IHorarioSalaService service, 
                                    IHardwareDeSalaService hardwareService, 
                                    ISalaService salaService, 
                                    IBlocoService blocoService, 
                                    IMonitoramentoService monitoramentoService)
        {
            _service = service;
            _hardwareService = hardwareService;
            _salaService = salaService;
            _blocoService = blocoService;
            _monitoramentoService = monitoramentoService;
        }

        [HttpGet]
        [Route("getReservasByUsuario/{diaSemana}/{idUsuario}")]
        public ActionResult GetReservasUsuario(string diaSemana, int idUsuario)
        {
            try
            {

                var salas = new SalaUsuarioViewModel();

                foreach (var item in _service.GetProximasReservasByIdUsuarioAndDiaSemana(idUsuario, diaSemana))
                {
                    var sala = _salaService.GetById(item.SalaId);
                    var bloco = _blocoService.GetById(sala.BlocoId);

                    salas.SalasUsuario.Add(new SalaUsuarioAuxModel
                    {
                        HorarioSala = item,
                        Sala = sala,
                        Bloco = bloco,
                        MonitoramentoLuzes = _monitoramentoService.GetByIdSalaAndTipoEquipamento(sala.Id, EquipamentoModel.TIPO_LUZES),
                        MonitoramentoCondicionadores = _monitoramentoService.GetByIdSalaAndTipoEquipamento(sala.Id, EquipamentoModel.TIPO_CONDICIONADOR)
                    });
                }

                return Ok(new {
                    result = salas,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Consulta realizada com sucesso"
                });
            } 
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }
        }

        [HttpDelete]
        [Route("cancelarReserva/{idReserva}")]
        public ActionResult CancelarReserva(int idReserva)
        {
            try
            {
                if (!_service.ConcelarReserva(idReserva))
                {
                    return BadRequest(new
                    {
                        result = false,
                        httpCode = (int)HttpStatusCode.BadRequest,
                        message = "Sua requisição não pode ser processada"
                    });
                }
                return Ok(new
                {
                    result = true,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Reserva cancelada com sucesso"
                });
            }
            catch (ServiceException se)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = se.Message
                });
            }
        }

        // GET: api/HorarioSala
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            try
            {
                var horarios = _service.GetAll();
                if (horarios.Count == 0)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Horário encontrado!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Horário encontrado com sucesso!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }
        }

        // GET: api/HorarioSala/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            try
            {
                var horario = _service.GetById(id);
                if (horario == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Horário encontrado!"
                    });

                return Ok(new
                {
                    result = horario,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Horário encontrado com sucesso!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDaSala/{idSala}")]
        [AllowAnonymous]
        public ActionResult GetReservasDaSala(int idSala)
        {
            try
            {
                var horarios = _service.GetByIdSala(idSala);
                if (horarios.Count == 0)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Horário encontrado!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Horário retornado com sucesso!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDaSemana/{idSala}")]
        [AllowAnonymous]
        public ActionResult GetReservasDaSamana(int idSala)
        {
            try
            {
                var horarios = _service.GetReservasDaSemanaByIdSala(idSala);
                if (horarios.Count == 0)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Reserva encontrada!"
                    });

                return Ok(new
                {
                    result = horarios,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Reservas retornadas com sucesso!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }
        }


        // GET: api/Hardware/5
        [HttpGet("{uuid}/get-horarios-sala")]
        [AllowAnonymous]
        public ActionResult GetHorarioSemana([FromRoute] string uuid, [FromQuery] string token)
        {

            try
            {
                var hardware = _hardwareService.GetByUuid(uuid);
                if (hardware == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Hardware não foi encontrado na base de dados com esse uuid",
                    });

                else if (string.IsNullOrEmpty(token) && !token.Equals(hardware.Token) && (string.IsNullOrEmpty(hardware.Token)))
                    return StatusCode((int)HttpStatusCode.Unauthorized, new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.Unauthorized,
                        message = "O token é inválido!"
                    });

                else if (hardware.Uuid == null)
                    return StatusCode((int)HttpStatusCode.Unauthorized, new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.Unauthorized,
                        message = "Erro, Hardware não está registrado!"
                    });
                else
                {
                    var horarios = _service.GetReservasDaSemanaByIdSala(hardware.SalaId);
                    if (horarios.Count > 0)
                        return Ok(new
                        {
                            result = new { schedules = horarios },
                            httpCode = (int)HttpStatusCode.OK,
                            message = "Horarios obtidos com sucesso",
                        });
                    else
                        return Ok(new
                        {
                            result = "null",
                            httpCode = (int)HttpStatusCode.BadRequest,
                            message = "Horarios não foram encontrados",
                        });
                }
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }
        }

        // GET: api/ReservaSala/5
        [HttpGet]
        [Route("ReservasDeHoje/{idSala}")]
        [AllowAnonymous]
        public ActionResult GetReservasDeHoje(int idSala)
        {
            try
            {
                var horarios = _service.GetReservasDeHojeByIdSala(idSala);
                
                if (horarios.Count == 0)
                    return Ok(new 
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
        [AllowAnonymous]
        public ActionResult GetReservasDeHojeByUuid(string uuid)
        {
            try
            {
                var horarios = _service.GetReservasDeHojeByUuid(uuid);

                if (horarios.Count == 0)
                    return Ok(new
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
                    return Ok(new
                    {
                        result = horarioSala,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Horário criado com sucesso!"
                    });

                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Houve um problema na inclusão do horário!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
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
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Houve um problema na atualização do horário!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                     new
                     {
                         result = "null",
                         httpCode = (int)HttpStatusCode.InternalServerError,
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
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Horário excluido com sucesso!"
                    });

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }
        }
    }
}
