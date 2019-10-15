using Models;
using Persistences;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class HardwareService : IService<HardwareModel>
    {
        private readonly ContextDb _context;
        public HardwareService(ContextDb context)
        {
            _context = context;
        }
        public List<HardwareModel> GetAll() => _context.Hardware.Select(h => new HardwareModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).ToList();

        public HardwareModel GetById(int id) => _context.Hardware.Where(h => h.Id == id).Select(h => new HardwareModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).FirstOrDefault();

        public bool Insert(HardwareModel entity)
        {
            _context.Add(SetEntity(entity, new Hardware()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Hardware.Where(tu => tu.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(HardwareModel entity)
        {
            var x = _context.Hardware.Where(tu => tu.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Hardware SetEntity(HardwareModel model, Hardware entity)
        {
            entity.Id = model.Id;
            entity.Mac = model.MAC;
            entity.TipoHardware = model.TipoHardwareId;
            entity.Sala = model.SalaId;

            return entity;
        }
    }
}
