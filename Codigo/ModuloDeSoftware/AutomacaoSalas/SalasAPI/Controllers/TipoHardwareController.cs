﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interface;
using System.Net;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET api/TipoHardware/6
        [HttpGet("{id}")]
        public ActionResult Get(uint id)
        {
            try
            {
                var tipo = _service.GetById(id);
                if (tipo == null)
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Tipo de Hardware não encontrado na base de dados."
                    });

                return Ok(tipo);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
