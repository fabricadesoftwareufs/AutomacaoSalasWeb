using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Net;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class EquipamentoController : ControllerBase
    {
        private readonly IEquipamentoService _service;
        public EquipamentoController(IEquipamentoService service)
        {
            _service = service;
        }

        // GET: api/Equipamento/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            try
            {
                var equipamentosSala = _service.GetByIdEquipamento(id);
                if (equipamentosSala == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Equipamento não encontrado na base de dados."
                    });

                return Ok(new
                {
                    result = equipamentosSala,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Equipamento obtido com sucesso!"
                });

            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        // GET: api/Equipamento/5
        [HttpGet("equipamentosSala/{id}")]
        [AllowAnonymous]
        public ActionResult GetEquipamentosSala(uint id)
        {
            try
            {
                var equipamentosSala = _service.GetByIdSala(id);
                if (equipamentosSala.Count == 0)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Euuipamentos não encontrados na base de dados!"
                    });

                return Ok(equipamentosSala);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        // GET: api/Equipamento/5
        [HttpGet("sala/{id}/tipoEquipamento/{tipo}")]
        [AllowAnonymous]
        public ActionResult GetTipoEquipamentoSala(int id, string tipo)
        {
            try
            {
                var equipamentosSala = _service.GetByIdSalaAndTipoEquipamento(id, tipo);
                if (equipamentosSala == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Equipamento não encontrado na base de dados!"
                    });

                return Ok(equipamentosSala);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        // POST: api/Equipamento
        [HttpPost]
        public ActionResult Post([FromBody] EquipamentoViewModel equipamentoModel)
        {
            try
            {
                // Obtém o ID do usuário do token de autenticação
                uint idUsuario = 0;
                if (User.Identity.IsAuthenticated && User.FindFirst("UserId") != null)
                {
                    uint.TryParse(User.FindFirst("UserId").Value, out idUsuario);
                }

                if (_service.Insert(equipamentoModel, idUsuario))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // PUT: api/Equipamento/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] EquipamentoViewModel salaParticularModel)
        {
            try
            {
                if (_service.Update(salaParticularModel))
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
        // GET: api/Equipamento
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAll()
        {
            try
            {
                var equipamentos = _service.GetAll();
                if (equipamentos == null || equipamentos.Count == 0)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum equipamento encontrado na base de dados."
                    });

                return Ok(new
                {
                    result = equipamentos,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Equipamentos obtidos com sucesso!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
