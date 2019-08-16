using Models;
using Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class UsuarioOrganizacoesService : IService<UsuarioOrganizacaoModel>
    {
        private readonly ContextDb _context;
        public UsuarioOrganizacoesService(ContextDb context)
        {
            _context = context;
        }
        public List<UsuarioOrganizacaoModel> GetAll() => _context.UsuarioOrganizacoes.Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.Usuario, OrganizacaoId = uo.Organizacao}).ToList();

        public UsuarioOrganizacaoModel GetById(int id) => _context.UsuarioOrganizacoes.Where(uo => uo.Id == id).Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.Usuario, OrganizacaoId = uo.Organizacao }).FirstOrDefault();

        public bool Insert(UsuarioOrganizacaoModel entity)
        {
            _context.Add(SetEntity(entity, new UsuarioOrganizacoes()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.UsuarioOrganizacoes.Where(uo => uo.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(UsuarioOrganizacaoModel entity)
        {
            var x = _context.UsuarioOrganizacoes.Where(uo => uo.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static UsuarioOrganizacoes SetEntity(UsuarioOrganizacaoModel model, UsuarioOrganizacoes entity)
        {
            entity.Id = model.Id;
            entity.Usuario = model.UsuarioId;
            entity.Organizacao = model.OrganizacaoId;

            return entity;
        }
    }
}
