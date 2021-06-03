using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizacaoController : ControllerBase
    {
        private readonly IOrganizacaoService _service;
        public OrganizacaoController(IOrganizacaoService service)
        {
            _service = service;
        }
        // GET: api/Organizacao
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var organizacoes = _service.GetAll();
                if (organizacoes.Count == 0)
                    return NoContent();

                return Ok(organizacoes);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/Organizacao/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var org = _service.GetById(id);
                if (org == null)
                    return NotFound("Organizacao não encontrada na base de dados");

                return Ok(org);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST: api/Organizacao
        [HttpPost]
        public ActionResult Post([FromBody] OrganizacaoModel organizacao)
        {
            try
            {
                if (ModelState.IsValid && _service.Insert(organizacao))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(ModelState);

        }

        // PUT: api/Organizacao/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] OrganizacaoModel organizacao)
        {

            try
            {
                if (ModelState.IsValid && _service.Update(organizacao))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest(ModelState);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_service.Remove(id))
                    return Ok();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

            return BadRequest();
        }
    }
}
