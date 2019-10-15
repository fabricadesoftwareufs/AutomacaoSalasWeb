using Models;
using Persistences;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class BlocoService : IService<BlocoModel>
    {
        private readonly ContextDb _context;
        public BlocoService(ContextDb context)
        {
            _context = context;
        }
        public List<BlocoModel> GetAll() => _context.Bloco.Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao }).ToList();

        public BlocoModel GetById(int id) => _context.Bloco.Where(b => b.Id == id).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao }).FirstOrDefault();

        public bool Insert(BlocoModel entity)
        {
            _context.Add(SetEntity(entity, new Bloco()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Bloco.Where(b => b.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(BlocoModel entity)
        {
            var x = _context.Bloco.Where(b => b.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Bloco SetEntity(BlocoModel model, Bloco entity)
        {
            entity.Id = model.Id;
            entity.Organizacao = model.OrganizacaoId;

            return entity;
        }
    }
}
