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
    public class TipoUsuarioController : ControllerBase
    {
        private readonly TipoUsuarioService _service;
        public TipoUsuarioController(TipoUsuarioService service)
        {
            _service = service;
        }
        // GET: api/TipoUsuario
        [HttpGet]
        public ActionResult<List<TipoUsuarioModel>> Get() => _service.GetAll();

        // GET: api/TipoUsuario/5
        [HttpGet("{id}")]
        public ActionResult<TipoUsuarioModel> Get(int id) => _service.GetById(id);

        // POST: api/TipoUsuario
        [HttpPost]
        public bool Post([FromBody] TipoUsuarioModel tipoUsuario) => _service.Insert(tipoUsuario);

        // PUT: api/TipoUsuario/5
        [HttpPut("{id}")]
        public bool Put([FromBody] TipoUsuarioModel tipoUsuario) => _service.Update(tipoUsuario);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
