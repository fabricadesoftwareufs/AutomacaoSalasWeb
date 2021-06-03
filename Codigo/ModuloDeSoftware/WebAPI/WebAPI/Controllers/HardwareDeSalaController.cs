using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HardwareDeSalaController : ControllerBase
    {
        private readonly IHardwareDeSalaService _service;
        public HardwareDeSalaController(IHardwareDeSalaService service)
        {
            _service = service;
        }
        // GET: api/Hardware
        [HttpGet]
        public ActionResult Get()
        {
            var hardwares = _service.GetAll();
            if (hardwares.Count == 0)
                return NoContent();

            return Ok(hardwares);
        }

        // GET: api/Hardware/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var hardware = _service.GetById(id);
            if (hardware == null)
                return NotFound("Hardware não encontrado na base de dados");

            return Ok(hardware);
        }

        // POST: api/Hardware
        [HttpPost]
        public ActionResult Post([FromBody] HardwareDeSalaModel hardwareModel, int idUser)
        {

            try
            {
                if (string.IsNullOrEmpty(hardwareModel.Ip) && hardwareModel.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                    ModelState.AddModelError("Ip", "Adicione um endereço IP");

                if (ModelState.IsValid && _service.Insert(hardwareModel,idUser))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(ModelState);
        }



        // PUT: api/Hardware/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] HardwareDeSalaModel hardwareModel, int idUser)
        {
            try
            {
                if (string.IsNullOrEmpty(hardwareModel.Ip) && hardwareModel.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                    ModelState.AddModelError("Ip", "Adicione um endereço IP");

                if (ModelState.IsValid && _service.Update(hardwareModel, idUser))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(hardwareModel);
        }



        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) 
        {
            try
            {
                if (_service.Remove(id))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest();
        }
    }
}
