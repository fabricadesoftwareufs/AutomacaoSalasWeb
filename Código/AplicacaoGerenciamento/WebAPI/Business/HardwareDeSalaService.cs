using Model;
using Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class HardwareDeSalaService : IService<HardwareDeSalaModel>
    {
        private readonly STR_DBContext _context;
        public HardwareDeSalaService(STR_DBContext context)
        {
            _context = context;
        }
        public List<HardwareDeSalaModel> GetAll() => _context.Hardwaredesala.Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).ToList();

        public HardwareDeSalaModel GetById(int id) => _context.Hardwaredesala.Where(h => h.Id == id).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).FirstOrDefault();

        public List<HardwareDeSalaModel> GetByIdSala(int id) => _context.Hardwaredesala.Where(h => h.Sala == id).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).ToList();
        public HardwareDeSalaModel GetByMAC(string mac) => _context.Hardwaredesala.Where(h => h.Mac.Equals(mac)).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).FirstOrDefault();


        public bool Insert(HardwareDeSalaModel entity)
        {
            _context.Add(SetEntity(entity, new Hardwaredesala()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Hardwaredesala.Where(tu => tu.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(HardwareDeSalaModel entity)
        {
            var x = _context.Hardwaredesala.Where(tu => tu.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Hardwaredesala SetEntity(HardwareDeSalaModel model, Hardwaredesala entity)
        {
            entity.Id = model.Id;
            entity.Mac = model.MAC;
            entity.TipoHardware = model.TipoHardwareId;
            entity.Sala = model.SalaId;

            return entity;
        }


        public List<HardwareDeSalaModel> GetSelectedList()
            => _context.Hardwaredesala.Select(s => new HardwareDeSalaModel { Id = s.Id, MAC = string.Format("{0} - {1}", s.Id, s.Mac) }).ToList();

    }
}
