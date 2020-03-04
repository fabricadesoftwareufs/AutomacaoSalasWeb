using Model;
using Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OrganizacaoService : IService<OrganizacaoModel>
    {
        private readonly STR_DBContext _context;
        public OrganizacaoService(STR_DBContext context)
        {
            _context = context;
        }

        public List<OrganizacaoModel> GetAll() => _context.Organizacao.Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).ToList();

        public OrganizacaoModel GetById(int id) => _context.Organizacao.Where(o => o.Id == id).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).FirstOrDefault();

        public bool Insert(OrganizacaoModel entity)
        {
            _context.Add(SetEntity(entity, new Organizacao()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Organizacao.Where(o => o.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(OrganizacaoModel entity)
        {
            var x = _context.Organizacao.Where(o => o.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Organizacao SetEntity(OrganizacaoModel model, Organizacao entity)
        {
            entity.Id = model.Id;
            entity.RazaoSocial = model.RazaoSocial;
            entity.Cnpj = model.Cnpj;

            return entity;
        }

        public List<OrganizacaoModel> GetSelectedList()
           => _context.Organizacao.Select(s => new OrganizacaoModel { Id = s.Id, RazaoSocial = string.Format("{0} - {1}", s.Id, s.RazaoSocial) }).ToList();

    }
}
