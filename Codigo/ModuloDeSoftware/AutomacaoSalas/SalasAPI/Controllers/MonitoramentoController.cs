using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Net;
using System.Security.Claims;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
    public class MonitoramentoController : ControllerBase
    {

        private readonly IMonitoramentoService _monitoramentoService;
        private readonly IUsuarioService _usuarioService;

        public MonitoramentoController(IMonitoramentoService service, IUsuarioService usuarioService)
        {
            _monitoramentoService = service;
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Atualiza o estado de um equipamento (ligado/desligado) e sincroniza com o hardware
        /// </summary>
        /// <param name="monitoramento"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("MonitorarSala/")]
        public ActionResult MonitorarSala(MonitoramentoViewModel monitoramento)
        {
            try
            {
               var monitoramentoRealizado = _monitoramentoService.MonitorarSala(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id, monitoramento);

                if (!monitoramentoRealizado)
                {
                    return BadRequest(new
                    {
                        result = monitoramentoRealizado.ToString(),
                        httpCode = (int)HttpStatusCode.BadRequest,
                        message = "Houve um problema ao realizar monitoramento!"
                    });
                }

                return Ok(new
                {
                    result = monitoramentoRealizado.ToString(),
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Monitoramento realizado com sucesso!"
                });

            }
            catch (ServiceException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = ex.Message
                });
            }
        }

        // GET: api/Monitoramento/5
        [HttpGet]
        [Route("ObterPorSala/{idSala}")]
        [AllowAnonymous]
        public ActionResult Get(int idEquipamento)
        {
            try
            {
                var monitoramento = _monitoramentoService.GetByIdEquipamento(idEquipamento);
                if (monitoramento == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Monitoramento encontrado!"
                    });

                return Ok(new {
                        result = monitoramento,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Monitoramento retornado com sucesso!"
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

        // GET: api/Monitoramento/5
        [HttpGet]
        [Route("obter-por-sala-tipo-equipamento/{idSala}/{tipoEquipamento}")]
        [AllowAnonymous]
        public ActionResult GetMonitoramentoByIdSala([FromRoute] uint idSala, [FromRoute] string tipoEquipamento)
        {
            try
            {
                var monitoramento = _monitoramentoService.GetByIdSalaAndTipoEquipamento(idSala, tipoEquipamento);

                if (monitoramento == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum Monitoramento encontrado!"
                    });

                return Ok(new
                {
                    result = monitoramento,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Monitoramento retornado com sucesso!"
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

        // PUT: api/Monitoramento/5
        [HttpPut]
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public ActionResult Atualizar([FromBody] MonitoramentoModel monitoramento)
        {
            try
            {
                uint idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;

                if (_monitoramentoService.Update(monitoramento, idUsuario))
                    return Ok(new
                    {
                        result = monitoramento,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Monitoramento atualizado!"
                    });

                return BadRequest(new
                {
                    result = monitoramento,
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Houve um problema ao atualizar monitoramento!"
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
    }
}
