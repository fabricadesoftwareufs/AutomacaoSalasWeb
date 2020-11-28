using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        //api/Utils
        [HttpGet]
        public ActionResult Get()
        {
            var hora = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return Ok(hora);
        }
        //getMillis - getDateTime 
        [HttpGet("{id}")]
        public ActionResult Get(String id)
        {
            String hora = "";
            if (id.ToUpper() == "GETMILLIS")
            {
                hora = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            }
            else if (id.ToUpper() == "GETDATETIME")
            {
                hora = DateTime.Now.ToString("HH:mm:ss;dd/MM/yyyy");
            }

            else if (id.ToUpper() == "GETTIME")
            {
                hora = DateTime.Now.ToString("HH:mm:ss");
            }
            else if (id.ToUpper() == "GETDATE")
            {
                hora = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (id.ToUpper() == "GETDATENEXTSUNDAY")
            {
                hora = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek).ToString("dd/MM/yyyy");
            }
            else if (id.ToUpper() == "GETDATEPREVIOUSSUNDAY")
            {
                hora = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToString("dd/MM/yyyy");
            }
            else
            {
                return BadRequest();
            }

            return Ok(hora);
        }
    }
}