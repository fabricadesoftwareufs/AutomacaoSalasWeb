using Model;
using Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class SalaService : IService<SalaModel>
    {
        private readonly STR_DBContext _context;
        public SalaService(STR_DBContext context)
        {
            _context = context;
        }
        public List<SalaModel> GetAll() => _context.Sala.Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).ToList();

        public SalaModel GetById(int id) => _context.Sala.Where(s => s.Id == id).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).FirstOrDefault();

        public bool Insert(SalaModel entity)
        {
            _context.Add(SetEntity(entity, new Sala()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Sala.Where(s => s.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(SalaModel entity)
        {
            var x = _context.Sala.Where(s => s.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Sala SetEntity(SalaModel model, Sala entity)
        {
            entity.Id = model.Id;
            entity.Titulo = model.Titulo;
            entity.Bloco = model.BlocoId;

            return entity;
        }
    }
}
