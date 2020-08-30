using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class UsuarioOrganizacaoService : IUsuarioOrganizacaoService
    {
        private readonly STR_DBContext _context;
        public UsuarioOrganizacaoService(STR_DBContext context)
        {
            _context = context;
        }
        public List<UsuarioOrganizacaoModel> GetAll() => _context.Usuarioorganizacao.Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.Usuario, OrganizacaoId = uo.Organizacao }).ToList();

        public UsuarioOrganizacaoModel GetById(int id) => _context.Usuarioorganizacao.Where(uo => uo.Id == id).Select(uo => new UsuarioOrganizacaoModel { Id = uo.Id, UsuarioId = uo.Usuario, OrganizacaoId = uo.Organizacao }).FirstOrDefault();

        public List<UsuarioOrganizacaoModel> GetByIdUsuario(int id) => _context.Usuarioorganizacao.Where(uo => uo.Usuario == id).Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.Usuario, OrganizacaoId = uo.Organizacao }).ToList();
        public List<UsuarioOrganizacaoModel> GetByIdOrganizacao(int id) => _context.Usuarioorganizacao.Where(uo => uo.Organizacao == id).Select(uo => new UsuarioOrganizacaoModel { UsuarioId = uo.Usuario, OrganizacaoId = uo.Organizacao }).ToList();

        public bool Insert(UsuarioOrganizacaoModel entity)
        {
            var usuario = SetEntity(entity);
            _context.Add(usuario);
            var ok = _context.SaveChanges();
            return ok == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Usuarioorganizacao.Where(uo => uo.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(UsuarioOrganizacaoModel entity)
        {
            var x = _context.Usuarioorganizacao.Where(uo => uo.Id == entity.Id).FirstOrDefault();
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
            Id = model.Id,
            Usuario = model.UsuarioId,
            Organizacao = model.OrganizacaoId

        };

        public bool RemoveByUsuario(int id)
        {
            var x = _context.Usuarioorganizacao.Where(uo => uo.Usuario == id);
            if (x != null)
            {
                _context.RemoveRange(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }
    }
}
