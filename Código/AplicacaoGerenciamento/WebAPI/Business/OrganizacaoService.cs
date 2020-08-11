using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OrganizacaoService : IOrganizacaoService
    {
        private readonly STR_DBContext _context;
        public OrganizacaoService(STR_DBContext context)
        {
            _context = context;
        }

        public List<OrganizacaoModel> GetAll() => _context.Organizacao.Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).ToList();

        public OrganizacaoModel GetById(int id) => _context.Organizacao.Where(o => o.Id == id).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).FirstOrDefault();

        public OrganizacaoModel GetByCnpj(string cnpj) => _context.Organizacao.Where(o => o.Cnpj.Equals(cnpj)).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).FirstOrDefault();

        public bool Insert(OrganizacaoModel entity)
        {
            try
            {
                if(GetByCnpj(entity.Cnpj) != null)
                    throw new ServiceException("Uma organização com esse cnpj já está cadastrada!");

                _context.Add(SetEntity(entity, new Organizacao()));

                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public bool Remove(int id)
        {
            var _blocoService = new BlocoService(_context);
            var _usuarioOrganizacaoService = new UsuarioOrganizacaoService(_context);
            try
            {
                if (_blocoService.GetByIdOrganizacao(id).Count > 0 && _usuarioOrganizacaoService.GetByIdOrganizacao(id).Count > 0)
                    throw new ServiceException("Organização não pode ser removida pois ainda existem usuários ou blocos associados a ela!");

                var x = _context.Organizacao.Where(o => o.Id == id).FirstOrDefault();
                if (x != null)
                {
                    _context.Remove(x);
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        public bool Update(OrganizacaoModel entity)
        {
            try
            {
                var empresa = GetByCnpj(entity.Cnpj);
                if (empresa != null && empresa.Id != entity.Id)
                    throw new ServiceException("Organização não pode ser removida pois ainda existem usuários ou blocos associados a ela!");

                var x = _context.Organizacao.Where(o => o.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                throw e;
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
    }
}
