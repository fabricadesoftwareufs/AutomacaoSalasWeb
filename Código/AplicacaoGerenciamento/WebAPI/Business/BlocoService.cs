using Model;
using Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class BlocoService : IService<BlocoModel>
    {
        private readonly STR_DBContext _context;
        public BlocoService(STR_DBContext context)
        {
            _context = context;
        }
        public List<BlocoModel> GetAll() => _context.Bloco.Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao, Titulo = b.Titulo }).ToList();

        public BlocoModel GetById(int id) => _context.Bloco.Where(b => b.Id == id).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao, Titulo = b.Titulo }).FirstOrDefault();

        public List<BlocoModel> GetByIdOrganizacao(int id) => _context.Bloco.Where(b => b.Organizacao == id).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao, Titulo = b.Titulo }).ToList();


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
            entity.Titulo = model.Titulo;

            return entity;
        }

        public List<BlocoModel> GetSelectedList()
           => _context.Bloco.Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} - {1}", s.Id, s.Titulo) }).ToList();
    }
}
