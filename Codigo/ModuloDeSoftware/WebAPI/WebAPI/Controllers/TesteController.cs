using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(
                new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.OK,
                    message = "Rota teste",
                }
            );
        }
    }
}
