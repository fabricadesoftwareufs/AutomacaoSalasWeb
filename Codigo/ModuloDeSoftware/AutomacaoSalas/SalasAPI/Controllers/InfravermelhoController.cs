using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class InfravermelhoController : ControllerBase
    {

        private readonly ICodigoInfravermelhoService _service;
        public InfravermelhoController(ICodigoInfravermelhoService service)
        {
            _service = service;
        }

        // GET api/<InfravermelhoController>/5
        [HttpGet]
        [Route("CodigosPorSala/{idSala}/{operacao}")]
        public ActionResult Get(int idSala, int operacao)
        {

            try
            {
                CodigoInfravermelhoModel codigos = _service.GetByIdSalaAndIdOperacao(idSala, operacao);
                if (codigos == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });

                return Ok(new
                {
                    result = codigos,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Códigos Obtidos com sucesso!"
                });
            }
            catch (ServiceException e)
            {
                return StatusCode(500, new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.InternalServerError,
                    message = e.Message
                });
            }
            
        }


        // GET api/<InfravermelhoController>/5
        [HttpGet("{idEquipamento}")]
        public ActionResult Get(int idEquipamento)
        {

            try
            {
                var codigos = _service.GetAllByEquipamento(idEquipamento);
                if (codigos == null || codigos.Count == 0)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });

                return Ok(new
                {
                    result = codigos,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Códigos Obtidos com sucesso!"
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

        // GET api/<InfravermelhoController>/5
        [HttpGet]
        [Route("CodigosPorUuid/{uuid}")]
        public ActionResult Get(string uuid)
        {

            try
            {
                var codigos = _service.GetAllByUuidHardware(uuid);
                if (codigos == null || codigos.Count == 0)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });

                return Ok(new
                {
                    result = codigos,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Códigos Obtidos com sucesso!"
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

        // POST api/<InfravermelhoController>
        [HttpPost]
        public ActionResult Post([FromBody] CodigoInfravermelhoModel codigoInfravermelhoModel)
        {
            try
            {
                var codigo = _service.Insert(codigoInfravermelhoModel);
                if (codigo != null)
                    return Ok(new
                    {
                        result = codigo,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Código cadastrado com sucesso!"
                    });
                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = "Houve um problema no cadastro!"
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
        // GET api/<InfravermelhoController>/CodigosPorModeloOperacao/5/1
        [HttpGet]
        [Route("CodigosPorModeloOperacao/{idModeloEquipamento}/{idOperacao}")]
        public ActionResult GetByModeloOperacao(int idModeloEquipamento, int idOperacao)
        {
            try
            {
                CodigoInfravermelhoModel codigo = _service.GetByIdOperacaoAndIdModeloEquipamento(idModeloEquipamento, idOperacao);
                if (codigo == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });

                return Ok(new
                {
                    result = codigo,
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Código obtido com sucesso!"
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
