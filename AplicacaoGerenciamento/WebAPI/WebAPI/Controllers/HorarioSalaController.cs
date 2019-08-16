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
    public class HorarioSalaController : ControllerBase
    {
        private readonly HorarioSalaService _service;
        public HorarioSalaController(HorarioSalaService service)
        {
            _service = service;
        }
        // GET: api/HorarioSala
        [HttpGet]
        public List<HorarioSalaModel> Get() => _service.GetAll();

        // GET: api/HorarioSala/5
        [HttpGet("{id}")]
        public HorarioSalaModel Get(int id) => _service.GetById(id);

        // POST: api/HorarioSala
        [HttpPost]
        public bool Post([FromBody] HorarioSalaModel horarioSala) => _service.Insert(horarioSala);

        // PUT: api/HorarioSala/5
        [HttpPut("{id}")]
        public bool Put([FromBody] HorarioSalaModel horarioSala) => _service.Update(horarioSala);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
