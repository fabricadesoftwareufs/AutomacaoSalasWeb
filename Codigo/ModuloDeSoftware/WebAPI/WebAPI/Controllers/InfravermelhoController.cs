 using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                    return StatusCode(200, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });

                return Ok(new
                {
                    result = codigos,
                    httpCode = 200,
                    message = "Códigos Obtidos com sucesso!"
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


        // GET api/<InfravermelhoController>/5
        [HttpGet("{idEquipamento}")]
        public ActionResult Get(int idEquipamento)
        {

            try
            {
                var codigos = _service.GetAllByEquipamento(idEquipamento);
                if (codigos == null)
                    return StatusCode(200, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });

                return Ok(new
                {
                    result = codigos,
                    httpCode = 200,
                    message = "Códigos Obtidos com sucesso!"
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

        // GET api/<InfravermelhoController>/5
        [HttpGet]
        [Route("CodigosPorUuid/{uuid}")]
        public ActionResult Get(string uuid)
        {

            try
            {
                var codigos = _service.GetAllByUuidHardware(uuid);
                if (codigos == null)
                    return StatusCode(200, new
                    {
                        result = "null",
                        httpCode = 204,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });

                return Ok(new
                {
                    result = codigos,
                    httpCode = 200,
                    message = "Códigos Obtidos com sucesso!"
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

    }
}
