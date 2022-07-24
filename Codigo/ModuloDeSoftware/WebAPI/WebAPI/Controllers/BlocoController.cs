using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocoController : ControllerBase
    {
        private readonly IBlocoService _service;
        public BlocoController(IBlocoService service)
        {
            _service = service;
        }

        // GET: api/Bloco
        [HttpGet]
        public ActionResult Get()
        {
            var blocos = _service.GetAll();
            if (blocos.Count == 0)
                return StatusCode(200,new
                {
                    result = "null",
                    httpCode = 204,
                    message = "Não há nenhum bloco cadastrado!"
                });


            return Ok(new
            {
                result = blocos,
                httpCode = 200,
                message = "Blocos obtidos com sucesso!"
            });
        }

        // GET: api/Bloco/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var bloco = _service.GetById(id);
            if (bloco == null)
                return StatusCode(200, new
                {
                    result = "null",
                    httpCode = 204,
                    message = "Bloco não encontrado na base de dados!"
                });

            return Ok(new
            {
                result = bloco,
                httpCode = 200,
                message = "Bloco retornado com sucesso!"
            });

        }

        // POST: api/Bloco
        [HttpPost]
        public ActionResult Post([FromBody] BlocoModel blocoModel)
        {
            try
            {
                var bloco = _service.Insert(blocoModel);
                if (bloco != null)
                    return Ok(new
                    {
                        result = bloco,
                        httpCode = 200,
                        message = "Bloco cadastrado com sucesso!"
                    });

                return BadRequest(new
                {
                    result = "null",
                    httpCode = 400,
                    message = "Houve um problema no cadastro!"
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

        // PUT: api/Bloco/5
        [HttpPut]
        public ActionResult Put([FromBody] BlocoModel blocoModel)
        {
            try
            {
                if (_service.Update(blocoModel))
                    return Ok(new
                    {
                        result = "null",
                        httpCode = 200,
                        message = "Bloco atualizado com sucesso!"
                    });

                return StatusCode(400, new
                {
                    result = "null",
                    httpCode = 400,
                    message = "Houve um problema na atualização do Bloco!"
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_service.Remove(id))
                    return Ok(new
                    {
                        result = "null",
                        httpCode = 200,
                        message = "Bloco removido com sucesso!"
                    });

                return BadRequest(new
                {
                    result = "null",
                    httpCode = 400,
                    message = "Houve um problema ao remover o bloco!"
                });
                ;
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
