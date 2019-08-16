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
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;
        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }
        // GET: api/Usuario
        [HttpGet]
        public List<UsuarioModel> Get() => _service.GetAll();

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public UsuarioModel Get(int id) => _service.GetById(id);

        // POST: api/Usuario
        [HttpPost]
        public bool Post([FromBody] UsuarioModel usuario) => _service.Insert(usuario);

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public bool Put([FromBody] UsuarioModel usuario) => _service.Update(usuario);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
