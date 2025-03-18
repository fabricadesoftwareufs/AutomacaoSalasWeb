using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Service;
using Service.Interface;
using System.Net;
using Utils;

namespace SalasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class ConexaoInternetSalaController : Controller
    {
        private readonly IConexaoInternetSalaService _service;

        public ConexaoInternetSalaController(IConexaoInternetSalaService service)
        {
            _service = service;
        }

        // GET: api/ConexaoInternetSala
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAll()
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

        // GET: api/ConexaoInternetSala/id/5
        [HttpGet("Sala/{idConexaoInternet:int}/{idSala:int}")]
        [AllowAnonymous]
        public ActionResult GetById(uint idConexaoInternet, uint idSala)
        {
            var ssid = _service.GetById(idConexaoInternet, idSala);
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

        // GET: api/ConexaoInternetSala/Conexao/5
        [HttpGet("Conexao/{idConexao:int}")]
        [AllowAnonymous]
        public ActionResult GetByIdConexaoInternet(uint idConexao)
        {
            var ssid = _service.GetByIdConexaoInternet(idConexao);
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

        // GET: api/ConexaoInternetSala/sala/5
        [HttpGet("sala/{idSala:int}")]
        [AllowAnonymous]
        public ActionResult GetBySalaOrdenadoPorPrioridade(uint idSala)
        {
            var ssid = _service.GetBySalaOrdenadoPorPrioridade(idSala);
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