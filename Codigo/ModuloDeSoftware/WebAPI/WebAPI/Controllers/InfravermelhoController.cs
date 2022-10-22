﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Net;

namespace WebAPI.Controllers
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
        [Route("CodigosPorUuid/{uuid}/{operacao}")]
        public ActionResult Get(string uuid, int operacao)
        {

            try
            {
                var codigos = _service.GetAllByUuidHardware(uuid, operacao);

                if (codigos == null)
                {
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhum código foi encontrado para a requisição!"
                    });
                }

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

    }
}
