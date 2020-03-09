using Model;
using Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class HardwareDeBlocoService : IService<HardwareDeBlocoModel>
    {
        private readonly STR_DBContext _context;
        public HardwareDeBlocoService(STR_DBContext context)
        {
            _context = context;
        }
        public List<HardwareDeBlocoModel> GetAll() => _context.Hardwaredebloco.Select(h => new HardwareDeBlocoModel { Id = h.Id, MAC = h.Mac, BlocoId = h.Bloco, TipoHardwareId = h.TipoHardware }).ToList();

        public HardwareDeBlocoModel GetById(int id) => _context.Hardwaredebloco.Where(h => h.Id == id).Select(h => new HardwareDeBlocoModel { Id = h.Id, MAC = h.Mac, BlocoId = h.Bloco, TipoHardwareId = h.TipoHardware }).FirstOrDefault();

        public bool Insert(HardwareDeBlocoModel entity)
        {
            _context.Add(SetEntity(entity, new Hardwaredebloco()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Hardwaredebloco.Where(tu => tu.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(HardwareDeBlocoModel entity)
        {
            var x = _context.Hardwaredebloco.Where(tu => tu.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Hardwaredebloco SetEntity(HardwareDeBlocoModel model, Hardwaredebloco entity)
        {
            entity.Id = model.Id;
            entity.Mac = model.MAC;
            entity.TipoHardware = model.TipoHardwareId;
            entity.Bloco = model.BlocoId;

            return entity;
        }

        public List<HardwareDeBlocoModel> GetSelectedList()
            => _context.Hardwaredebloco.Select(s => new HardwareDeBlocoModel { Id = s.Id, MAC = string.Format("{0} - {1}", s.Id, s.Mac) }).ToList();
    }
}
