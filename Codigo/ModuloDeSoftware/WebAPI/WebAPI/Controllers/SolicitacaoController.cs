using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitacaoController : ControllerBase
    {
        private readonly ISolicitacaoService _solicitacaoService;

        public SolicitacaoController(ISolicitacaoService solicitacaoService)
        {
            _solicitacaoService = solicitacaoService;
        }

        /// <summary>
        /// Get Retorna as solicitações pelo idHardware e tipo, podendo filtrar entre todos registros ou nao.
        /// </summary>
        /// <param name="idHardware"></param>
        /// <param name="tipo"></param>
        /// <param name="todosRegistros"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get([FromQuery]int idHardware, [FromQuery]string tipo = "", [FromQuery]bool todosRegistros = false)
        {
            try
            {
                var solicitacoes = _solicitacaoService.GetByIdHardware(idHardware, tipo, todosRegistros);
                return Ok(solicitacoes);
            } catch(Exception)
            {
                return BadRequest();
            }
        }
    }
}
