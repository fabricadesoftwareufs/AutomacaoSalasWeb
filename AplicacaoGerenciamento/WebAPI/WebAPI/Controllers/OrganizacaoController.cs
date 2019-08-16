using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

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
        public List<OrganizacaoModel> Get() => _service.GetAll();

        // GET: api/Organizacao/5
        [HttpGet("{id}")]
        public OrganizacaoModel Get(int id) => _service.GetById(id);

        // POST: api/Organizacao
        [HttpPost]
        public bool Post([FromBody] OrganizacaoModel organizacao) => _service.Insert(organizacao);

        // PUT: api/Organizacao/5
        [HttpPut("{id}")]
        public bool Put([FromBody] OrganizacaoModel organizacao) => _service.Update(organizacao);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
