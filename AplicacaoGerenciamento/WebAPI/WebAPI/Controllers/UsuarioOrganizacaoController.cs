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
    public class UsuarioOrganizacaoController : ControllerBase
    {
        private readonly UsuarioOrganizacoesService _service;
        public UsuarioOrganizacaoController(UsuarioOrganizacoesService service)
        {
            _service = service;
        }
        // GET: api/UsuarioOrganizacao
        [HttpGet]
        public List<UsuarioOrganizacaoModel> Get() => _service.GetAll();

        // GET: api/UsuarioOrganizacao/5
        [HttpGet("{id}", Name = "Get")]
        public UsuarioOrganizacaoModel Get(int id) => _service.GetById(id);

        // POST: api/UsuarioOrganizacao
        [HttpPost]
        public bool Post([FromBody] UsuarioOrganizacaoModel usuarioOrganizacao) => _service.Insert(usuarioOrganizacao);

        // PUT: api/UsuarioOrganizacao/5
        [HttpPut("{id}")]
        public bool Put([FromBody] UsuarioOrganizacaoModel usuarioOrganizacao) => _service.Update(usuarioOrganizacao);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
