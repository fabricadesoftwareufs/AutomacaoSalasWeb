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
    public class HardwareController : ControllerBase
    {
        private readonly HardwareService _service;
        public HardwareController(HardwareService service)
        {
            _service = service;
        }
        // GET: api/Hardware
        [HttpGet]
        public IEnumerable<HardwareModel> Get() => _service.GetAll();

        // GET: api/Hardware/5
        [HttpGet("{id}")]
        public HardwareModel Get(int id) => _service.GetById(id);

        // POST: api/Hardware
        [HttpPost]
        public bool Post([FromBody] HardwareModel hardwareModel) => _service.Insert(hardwareModel);

        // PUT: api/Hardware/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] HardwareModel hardwareModel) => _service.Update(hardwareModel);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
