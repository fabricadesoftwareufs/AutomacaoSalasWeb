using Microsoft.AspNetCore.Mvc;
using Service.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
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

       /* // GET api/<InfravermelhoController>/5
        [HttpGet]
        [Route("CodigosPorEquipamentoSala/{idSala}")]
        public ActionResult Get(int idSala)
        {
            return Ok(_service.GetByIdSala(idSala));
        }*/
    }
}
