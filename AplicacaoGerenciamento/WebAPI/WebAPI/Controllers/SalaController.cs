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
    public class SalaController : ControllerBase
    {
        private readonly SalaService _service;
        public SalaController(SalaService service)
        {
            _service = service;
        }
        // GET: api/Sala
        [HttpGet]
        public List<SalaModel> Get() => _service.GetAll();

        // GET: api/Sala/5
        [HttpGet("{id}")]
        public SalaModel Get(int id) => _service.GetById(id);

        // POST: api/Sala
        [HttpPost]
        public bool Post([FromBody] SalaModel salaModel) => _service.Insert(salaModel);

        // PUT: api/Sala/5
        [HttpPut("{id}")]
        public bool Put([FromBody] SalaModel salaModel) => _service.Update(salaModel);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
