using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Service;
using Service.Interface;
using System.Net;
using Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class HardwareDeSalaController : ControllerBase
    {
        private readonly IHardwareDeSalaService _service;
        public HardwareDeSalaController(IHardwareDeSalaService service)
        {
            _service = service;
        }
        // GET: api/Hardware
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            var hardwares = _service.GetAll();
            if (hardwares.Count == 0)
                return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Não há nenhum hardware cadastrado!"
                    });

            return Ok(new
            {
                result = hardwares,
                httpCode = (int)HttpStatusCode.OK,
                message = "Hardwares(s) obtido(s) com sucesso!"
            });
        }

        // GET: api/Hardware/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            var hardware = _service.GetById(id);
            if (hardware == null)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Hardware não encontrado na base de dados",
                });

            else if (hardware.Uuid == null)
                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Hardware não está registrado!"
                });   

                
            
            return Ok(new
            {
                result = hardware,
                httpCode = (int)HttpStatusCode.OK,
                message = "Hardware obtido com sucesso!"
            });
        }

        // GET: api/Hardware/5
        [HttpGet("info/{mac}")]
        [AllowAnonymous]
        public ActionResult Get([FromRoute]string mac, [FromQuery]string token, [FromQuery(Name = "tipo-hardware")] int tipoHardware)
        {
            var hardware = _service.GetByMAC(mac);

            if (hardware == null)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Hardware não encontrado na base de dados",
                });
            else if (!token.Equals(hardware.Token) && (string.IsNullOrEmpty(hardware.Token) && !token.Equals(Methods.TOKEN_PADRAO)))
                return StatusCode((int)HttpStatusCode.Unauthorized, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.Unauthorized,
                    message = "O token é inválido!"
                });

            else if (hardware.TipoHardwareId != tipoHardware)
                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Não há match de hardware para essa requisição!"
                });

            return Ok(new
            {
                result = hardware,
                httpCode = (int)HttpStatusCode.OK,
                message = "Hardware obtido com sucesso!"
            });
        }

        // POST: api/Hardware
        [HttpPost]
        public ActionResult Post([FromBody] HardwareDeSalaModel hardwareModel, int idUser)
        {

            try
            {
                if (string.IsNullOrEmpty(hardwareModel.Ip) && hardwareModel.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                    ModelState.AddModelError("Ip", "Adicione um endereço IP");

                if (_service.Insert(hardwareModel, idUser))
                    return StatusCode((int)HttpStatusCode.Created, new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.Created,
                        message = "Hardware cadastrado com sucesso!"
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



        // PUT: api/Hardware/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] HardwareDeSalaModel hardwareModel, int idUser)
        {
            try
            {
                if (string.IsNullOrEmpty(hardwareModel.Ip) && hardwareModel.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                    ModelState.AddModelError("Ip", "Adicione um endereço IP");

                if (_service.Update(hardwareModel, idUser))
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Hardware atualizado com sucesso!"
                    });

                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "O Hardware não está existe ou houve problema na montagem da requisição!"
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
                        message = "Hardware removido com sucesso!"
                    });

                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Não foi possível remover o hardware solicitado!"
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

        [HttpPost("Register")]
        [AllowAnonymous]
        public ActionResult Register([FromBody] RegisterHardware registerHardware)
        {
            try
            {
                var hardware = (registerHardware.Id > 0) && (registerHardware.TipoHardwareId > 0) ? _service.GetByIdAndType(registerHardware.Id, registerHardware.TipoHardwareId) : null;

                if (hardware == null)
                   return StatusCode((int)HttpStatusCode.BadRequest, new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.BadRequest,
                        message = "Não há hardware cadastrado para essa requisição!"
                    });
                else if ((registerHardware!.Token != null && hardware!.Token != null) && (!registerHardware.Token.Equals(hardware.Token) && !registerHardware.Token.Equals(Methods.TOKEN_PADRAO)))
                    return StatusCode((int)HttpStatusCode.Unauthorized, new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.Unauthorized,
                        message = "O token é inválido!"
                    });

                else if (hardware.Registrado)
                    return
                    Ok(new
                    {
                        result = hardware,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Hardware já tem um registro!"
                    });


                hardware.Registrado = true;

                return _service.Update(hardware) ? 
                    Ok(new { result = hardware,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Hardware atualizado e registrado com sucesso!"
                    }) : 
                StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = "Não foi possível atualizar o hardware!"
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

        // GET: Master by UUID of slave
        [HttpGet("slave/{uuid}/get-master")]
        [AllowAnonymous]
        public ActionResult GetMaster([FromRoute] string uuid, [FromQuery] string token)
        {
            var hardware = _service.GetByUuid(uuid);
            if (hardware == null)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Hardware sensor não foi encontrado na base de dados",
                });
          
            else if (string.IsNullOrEmpty(token) && !token.Equals(hardware.Token) && (string.IsNullOrEmpty(hardware.Token) || !token.Equals(Methods.TOKEN_PADRAO)))
                return StatusCode((int)HttpStatusCode.Unauthorized, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.Unauthorized,
                    message = "O token é inválido!"
                });

            else if (hardware.Uuid == null)
                return StatusCode((int)HttpStatusCode.BadRequest, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Hardware não está registrado!"
                });

            else
            {
                var listHardwareControlador = _service.GetByIdSalaAndTipoHardware(hardware.SalaId, (int)HardwareDeSalaModel.TIPO.CONTROLADOR_SALA);
                HardwareDeSalaModel master;
                if (listHardwareControlador.Count > 0)
                {
                    master = listHardwareControlador[0];
                    if (master != null && master.Uuid == null)
                    {
                        return BadRequest(new
                        {
                            result = "null",
                            httpCode = (int)HttpStatusCode.BadRequest,
                            message = "O Controlador de Sala (master) não está registrado!"
                        });
                    }
                    else if (master == null)
                        return BadRequest(new
                        {
                            result = "null",
                            httpCode = (int)HttpStatusCode.BadRequest,
                            message = "Erro. O Controlador de Sala (master) não foi encontrado!"
                        });
                    else
                    {
                        return Ok(new
                        {
                            result = new { uuid = master.Uuid, mac = master.MAC },
                            httpCode = (int)HttpStatusCode.OK,
                            message = "Controlador de Sala (master) obtido com sucesso!"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.BadRequest,
                        message = "Erro crasso. O Controlador de Sala (master) não foi encontrado!"
                    });
                }
            }
             
        }

        // GET: api/Hardware/5
        [HttpGet("{idSala}/get-sensors-and-actuators")]
        [AllowAnonymous]
        public ActionResult GetSensorsAndActuatorsByIdSala([FromRoute] int idSala, [FromQuery] string token)
        {
            if (!Methods.TOKEN_PADRAO.Equals(token))
                return StatusCode((int)HttpStatusCode.Unauthorized, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.Unauthorized,
                    message = "O token é inválido!"
                });

            var hardware = _service.GetSensorsAndActuactorsByIdSala(idSala);

            return StatusCode((int)HttpStatusCode.OK, new
            {
                result = hardware,
                httpCode = (int)HttpStatusCode.OK,
                message = "Hardwares obtidos com sucesso",
            });         
        }

        // GET: api/Hardware/5
        [HttpGet("master/{uuid}/get-sensors")]
        [AllowAnonymous]
        public ActionResult GetSensors([FromRoute] string uuid, [FromQuery] string token)
        {
            var hardware = _service.GetByUuid(uuid);
            if (hardware == null)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Hardware master não foi encontrado na base de dados",
                });

            else if (!token.Equals(hardware.Token) && (string.IsNullOrEmpty(hardware.Token) || !token.Equals(Methods.TOKEN_PADRAO)))
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
                    message = "Hardware master não está registrado!"
                });

            else
            {
                var listHardwareControlador = _service.GetByIdSalaAndTipoHardware(hardware.SalaId, (int)HardwareDeSalaModel.TIPO.MODULO_SENSOR);
                if (listHardwareControlador.Count > 0)
                {
                    return Ok(new
                    {
                        result = new { sensors = listHardwareControlador },
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Os sensores (slaves) do Controlador de Sala (master) foram obtidos com sucesso!"
                    });
       
                }
                else
                {
                    return BadRequest(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.BadRequest,
                        message = "Erro crasso. Os sensores para esse master não foram encontrados!"
                    });
                }
            }

        }
    }
}
