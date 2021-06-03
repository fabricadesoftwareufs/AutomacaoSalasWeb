using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoHardwareController : ControllerBase
    {
        private readonly ITipoHardwareService _service;
        public TipoHardwareController(ITipoHardwareService service)
        {
            _service = service;
        }

        // GET api/TipoHardware
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var tipoHard = _service.GetAll();
                if (tipoHard.Count == 0)
                    return NoContent();

                return Ok(tipoHard);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET api/TipoHardware/6
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var tipo = _service.GetById(id);
                if (tipo == null)
                    return NotFound("Tipo de Hardware não encontrado na base de dados.");

                return Ok(tipo);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
