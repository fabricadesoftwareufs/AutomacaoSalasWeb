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
    public class OperacaoController : ControllerBase
    {
        private readonly IOperacaoCodigoService _service;
        public OperacaoController(IOperacaoCodigoService service)
        {
            _service = service;
        }

        // GET: api/Operacao
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var tipoUser = _service.GetAll();
                if (tipoUser.Count == 0)
                    return NoContent();

                return Ok(tipoUser);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
           
        }

        // GET: api/Operacao/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var tipo = _service.GetById(id);
                if (tipo == null)
                    return NotFound("Operacao não encontrado na base de dados.");

                return Ok(tipo);
            }
            catch (ServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
           
        }
    }
}
