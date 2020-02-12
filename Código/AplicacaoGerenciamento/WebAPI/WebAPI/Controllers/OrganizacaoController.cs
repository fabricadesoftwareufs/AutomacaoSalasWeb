using Service;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizacaoController : ControllerBase
    {
        private readonly OrganizacaoService _service;
        public OrganizacaoController(OrganizacaoService service)
        {
            _service = service;
        }
        // GET: api/Organizacao
        [HttpGet]
        public ActionResult Get()
        {
            var organizacoes = _service.GetAll();
            if (organizacoes.Count == 0)
                return NoContent();

            return Ok(organizacoes);
        }

        // GET: api/Organizacao/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var org = _service.GetById(id);
            if (org == null)
                return NoContent();

            return Ok(org);
        }

        // POST: api/Organizacao
        [HttpPost]
        public ActionResult Post([FromBody] OrganizacaoModel organizacao) => _service.Insert(organizacao) ? Ok(true) : Ok(false);

        // PUT: api/Organizacao/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] OrganizacaoModel organizacao) => _service.Update(organizacao) ? Ok(true) : Ok(false);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) => _service.Remove(id) ? Ok(true) : Ok(false);
    }
}
