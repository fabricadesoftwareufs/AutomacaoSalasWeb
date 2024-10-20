using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Net;
using System.Security.Claims;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ADMINISTRATIVE_ROLES)]
    public class SalaParticularController : ControllerBase
    {
        private readonly ISalaParticularService _service;
        private readonly ISalaService _salaService;
        private readonly IBlocoService _blocoService;
        private readonly IMonitoramentoService _monitoramentoService;
        public SalaParticularController(ISalaParticularService service,
                                    ISalaService salaService,
                                    IBlocoService blocoService,
                                    IMonitoramentoService monitoramentoService)
        {
            _service = service;
            _salaService = salaService;
            _blocoService = blocoService;
            _monitoramentoService = monitoramentoService;
        }
        // GET: api/SalaParticular
        [HttpGet]
        [AllowAnonymous]
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

        [HttpGet]
        [AllowAnonymous]
        [Route("getSalasExclusivasByUsuario/{idUsuario}")]
        public ActionResult GetSalasUsuario(uint idUsuario)
        {
            try
            {
                var salas = new SalaUsuarioViewModel();
                foreach (var item in _service.GetByIdUsuario(idUsuario))
                {
                    var sala = _salaService.GetById(item.SalaId);
                    var bloco = _blocoService.GetById(sala.BlocoId);

                    salas.SalasUsuario.Add(new SalaUsuarioAuxModel
                    {
                        SalaExclusiva = item,
                        Sala = sala,
                        Bloco = bloco,
                        MonitoramentoLuzes = _monitoramentoService.GetByIdSalaAndTipoEquipamento(sala.Id, EquipamentoModel.TIPO_LUZES),
                        MonitoramentoCondicionadores = _monitoramentoService.GetByIdSalaAndTipoEquipamento(sala.Id, EquipamentoModel.TIPO_CONDICIONADOR)
                    });
                }

                return Ok(new
                {
                    result = salas,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Consulta realizada com sucesso"
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

        // GET: api/SalaParticular/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult Get(uint id)
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
        public ActionResult Delete(uint id)
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
