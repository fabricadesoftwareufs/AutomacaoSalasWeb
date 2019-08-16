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
    public class DisciplinaController : ControllerBase
    {
        private readonly DisciplinaService _service;
        public DisciplinaController(DisciplinaService service)
        {
            _service = service;
        }
        // GET: api/Disciplina
        [HttpGet]
        public List<DisciplinaModel> Get() => _service.GetAll();

        // GET: api/Disciplina/5
        [HttpGet("{id}")]
        public DisciplinaModel Get(int id) => _service.GetById(id);

        // POST: api/Disciplina
        [HttpPost]
        public bool Post([FromBody] DisciplinaModel disciplina) => _service.Insert(disciplina);

        // PUT: api/Disciplina/5
        [HttpPut("{id}")]
        public bool Put([FromBody] DisciplinaModel disciplina) => _service.Update(disciplina);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
