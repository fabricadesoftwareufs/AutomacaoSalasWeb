using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;
using System;
using System.Net;

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
                if(solicitacoes.Count > 0)
                {
                    return Ok(new
                    {
                        result = solicitacoes,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Lista de solicitações retornadas com sucesso.",
                    });
                } else
                {
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.NoContent,
                        message = "Nenhuma solicitação foi encontrada na base de dados.",
                    });
                }
            } catch(Exception e)
            {
                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = e.Message,
                });
            }
        }


        /// <summary>
        /// Put Atualiza a solicitação
        /// </summary>
        /// <param name="solicitacao"></param>
        /// <returns></returns>
        [HttpPut("finalizar/{id}")]
        public ActionResult Finalizar(int id, [FromBody]DateTime dataFinalizacao)
        {
            try
            {
                var solicitacaoDB = _solicitacaoService.GetById(id);
                if(solicitacaoDB.DataFinalizacao.HasValue)
                {
                    return Ok(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.OK,
                        message = "A soliciação já foi finalizada anteriormente.",
                    });
                } 
                solicitacaoDB.DataFinalizacao = dataFinalizacao;

                var updated = _solicitacaoService.Update(solicitacaoDB);
                if(updated)
                {
                    return Ok(new
                    {
                        result = updated,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Solicitação foi finalizada com sucesso.",
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.BadRequest,
                        message = "Erro ao atualizar solicitação.",
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = e.Message,
                });
            }
        }

        /// <summary>
        /// Put Atualiza a solicitação
        /// </summary>
        /// <param name="id"></param>
        /// <param name="solicitacao"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SolicitacaoModel solicitacao)
        {
            try
            {
                var solicitacaoDB = _solicitacaoService.GetById(id);

                solicitacaoDB = UpdateSolicitacao(solicitacao, solicitacaoDB);

                var updated = _solicitacaoService.Update(solicitacaoDB);
                if (updated)
                {
                    return Ok(new
                    {
                        result = updated,
                        httpCode = (int)HttpStatusCode.OK,
                        message = "Solicitação foi finalizada com sucesso.",
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        result = "null",
                        httpCode = (int)HttpStatusCode.BadRequest,
                        message = "Erro ao atualizar solicitação.",
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    result = "null",
                    httpCode = (int)HttpStatusCode.BadRequest,
                    message = e.Message,
                });
            }
        }

        private SolicitacaoModel UpdateSolicitacao(SolicitacaoModel solicitacaoRequest, SolicitacaoModel solicitacaoDB)
        {
            solicitacaoDB.Payload = solicitacaoRequest.Payload;
            solicitacaoDB.DataSolicitacao = solicitacaoRequest.DataSolicitacao;
            solicitacaoDB.DataFinalizacao = solicitacaoRequest.DataFinalizacao;
            solicitacaoDB.TipoSolicitacao = solicitacaoRequest.TipoSolicitacao;
            solicitacaoDB.IdHardware = solicitacaoRequest.IdHardware;

            return solicitacaoDB;
        }
    }
}
