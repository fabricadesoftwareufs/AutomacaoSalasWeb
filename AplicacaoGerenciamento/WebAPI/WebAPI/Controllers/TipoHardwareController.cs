using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoHardwareController : ControllerBase
    {
        private readonly TipoHardwareService _service;
        public TipoHardwareController(TipoHardwareService service)
        {
            _service = service;
        }

        // GET api/TipoHardware
        [HttpGet]
        public ActionResult<List<TipoHardwareModel>> Get() => _service.GetAll();

        // GET api/TipoHardware/6
        [HttpGet("{id}")]
        public ActionResult<TipoHardwareModel> Get(int id) => _service.GetById(id);

        // POST api/TipoHardware
        [HttpPost]
        public bool Post([FromBody] TipoHardwareModel tipoHardware) => _service.Insert(tipoHardware);

        // PUT api/TipoHardware/5
        [HttpPut("{id}")]
        public bool Put([FromBody] TipoHardwareModel tipoHardware) => _service.Update(tipoHardware);

        // DELETE api/TipoHardware/5
        [HttpDelete("{id}")]
        public bool Delete(int id) => _service.Remove(id);
    }
}
