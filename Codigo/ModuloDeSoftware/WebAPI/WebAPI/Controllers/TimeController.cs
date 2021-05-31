using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private const string GETMILLIS = "GETMILLIS";
        private const string GETDATETIME = "GETDATETIME";
        private const string GETTIME = "GETTIME";
        private const string GETDATE = "GETDATE";
        private const string GETDATENEXTSUNDAY = "GETDATENEXTSUNDAY";
        private const string GETDATEPREVIOUSSUNDAY = "GETDATEPREVIOUSSUNDAY";


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
            String hora;
            id = id.Trim().ToUpper();

            if (id.Equals(GETMILLIS))
            {
                hora = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            }
            else if (id.Equals(GETDATETIME))
            {
                hora = GetHorarioBrasilia().ToString("HH:mm:ss;yyyy-MM-dd");
            }

            else if (id.Equals(GETTIME))
            {
                hora = GetHorarioBrasilia().ToString("HH:mm:ss");
            }
            else if (id.Equals(GETDATE))
            {
                hora = GetHorarioBrasilia().ToString("yyyy-MM-dd");
            }
            else if (id.Equals(GETDATENEXTSUNDAY))
            {
                hora = GetHorarioBrasilia().AddDays(7 - (int)DateTime.Now.DayOfWeek).ToString("yyyy-MM-dd");
            }
            else if (id.Equals(GETDATEPREVIOUSSUNDAY))
            {
                hora = GetHorarioBrasilia().AddDays(-(int)DateTime.Now.DayOfWeek).ToString("yyyy-MM-dd");
            }
            else
            {
                return BadRequest();
            }

            return Ok(hora);
        }

        private DateTime GetHorarioBrasilia() => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

    }
}