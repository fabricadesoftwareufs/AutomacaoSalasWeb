using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Service.Interface;
using System.Net;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class ConexaoInternetController : Controller
    {
        private readonly IConexaoInternetService _service;
     
        public ConexaoInternetController(IConexaoInternetService service)
        {
            _service = service;
        }

        // GET: api/ConexaoInternet
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            var ssid = _service.GetAll();
            if (ssid.Count == 0)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Não há nenhum ponto de acesso cadastrado!"
                });

            return Ok(new
            {
                result = ssid,
                httpCode = (int)HttpStatusCode.OK,
                message = "Ponto(s) de Acesso(s) obtido(s) com sucesso!"
            });
        }

        // GET: api/ConexaoInternet/id/5
        [HttpGet("id/{id:int}")]
        [AllowAnonymous]
        public ActionResult GetById(uint id)
        {
            var ssid = _service.GetById(id);
            if (ssid == null)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Ponto de Acesso não encontrado na base de dados",
                });

            return Ok(new
            {
                result = ssid,
                httpCode = (int)HttpStatusCode.OK,
                message = "Ponto de Acesso obtido com sucesso!"
            });
        }

        // GET: api/ConexaoInternet/name/{name}
        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public ActionResult GetByName(string name)
        {
            var ssid = _service.GetByName(name);
            if (ssid == null)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Ponto de Acesso não encontrado na base de dados",
                });

            return Ok(new
            {
                result = ssid,
                httpCode = (int)HttpStatusCode.OK,
                message = "Ponto de Acesso obtido com sucesso!"
            });
        }

        // Método para obter pontos de acesso por ID do bloco
        // GET: api/ConexaoInternet/bloco/{idBloco}
        [HttpGet("bloco/{idBloco:int}")]
        [AllowAnonymous]
        public ActionResult GetByIdBloco(uint idBloco)
        {
            var ssid = _service.GetByIdBloco(idBloco);
            if (ssid == null || ssid.Count == 0)
                return Ok(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.NoContent,
                    message = "Nenhum Ponto de Acesso encontrado para este bloco",
                });
            return Ok(new
            {
                result = ssid,
                httpCode = (int)HttpStatusCode.OK,
                message = "Pontos de Acesso obtidos com sucesso!"
            });
        }
    }
}