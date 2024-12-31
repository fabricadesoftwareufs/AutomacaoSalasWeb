using Microsoft.VisualBasic.FileIO;
using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class UsuarioOrganizacaoService : IUsuarioOrganizacaoService
    {
        private readonly SalasDBContext _context;
        public UsuarioOrganizacaoService(SalasDBContext context)
        {
            _context = context;
        }
        public List<UsuarioOrganizacaoModel> GetAll() => _context.Usuarioorganizacaos.Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.IdUsuario, OrganizacaoId = uo.IdOrganizacao }).ToList();

        public UsuarioOrganizacaoModel GetById(uint idUsuario, uint idOrganicao) => _context.Usuarioorganizacaos.Where(uo => uo.IdUsuario == idUsuario && uo.IdOrganizacao == idOrganicao).Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.IdUsuario, OrganizacaoId = uo.IdOrganizacao }).FirstOrDefault();

        public List<UsuarioOrganizacaoModel> GetByIdUsuario(uint id) => _context.Usuarioorganizacaos.Where(uo => uo.IdUsuario == id).Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.IdUsuario, OrganizacaoId = uo.IdOrganizacao }).ToList();
        public List<UsuarioOrganizacaoModel> GetByIdOrganizacao(uint id) => _context.Usuarioorganizacaos.Where(uo => uo.IdOrganizacao == id).Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.IdUsuario, OrganizacaoId = uo.IdOrganizacao }).ToList();

        public bool Insert(UsuarioOrganizacaoModel entity)
        {
            var usuario = SetEntity(entity);
            _context.Add(usuario);
            var ok = _context.SaveChanges();
            return ok == 1 ? true : false;
        }

        public bool Remove(uint idUsuario, uint idOrganizacao)
        {
            var x = _context.Usuarioorganizacaos.Where(uo => uo.IdUsuario == idUsuario && uo.IdOrganizacao == idOrganizacao).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(UsuarioOrganizacaoModel entity)
        {
            var x = _context.Usuarioorganizacaos.Where(uo => uo.IdUsuario == entity.UsuarioId && uo.IdOrganizacao == entity.OrganizacaoId).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Usuarioorganizacao SetEntity(UsuarioOrganizacaoModel model)
        => new Usuarioorganizacao()
        {
            IdUsuario = model.UsuarioId,
            IdOrganizacao = model.OrganizacaoId

        };

        public bool RemoveByUsuario(uint idUsuario)
        {
            var x = _context.Usuarioorganizacaos.Where(uo => uo.IdUsuario == idUsuario);
            if (x != null)
            {
                _context.RemoveRange(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }
    }
}
