using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly SalasDBContext _context;

        public TipoUsuarioService(SalasDBContext context)
        {
            _context = context;
        }

        public List<TipoUsuarioModel> GetAll() => _context.Tipousuarios
            .Select(tu => new TipoUsuarioModel { Id = tu.Id, Descricao = tu.Descricao })
            .ToList();

        public TipoUsuarioModel GetTipoUsuarioByUsuarioId(uint idUsuario)
        {
            // Busca o tipo de usuário a partir da tabela Usuarioorganizacao
            return _context.Usuarioorganizacaos
                .Where(uo => uo.IdUsuario == idUsuario)
                .Select(uo => new TipoUsuarioModel
                {
                    Id = uo.IdTipoUsuarioNavigation.Id,
                    Descricao = uo.IdTipoUsuarioNavigation.Descricao
                })
                .FirstOrDefault();
        }

        public bool Insert(TipoUsuarioModel entity)
        {
            _context.Add(SetEntity(entity, new Tipousuario()));
            return _context.SaveChanges() == 1;
        }

        public bool Remove(int id)
        {
            var x = _context.Tipousuarios.FirstOrDefault(tu => tu.Id == id);
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1;
            }

            return false;
        }

        public bool Update(TipoUsuarioModel entity)
        {
            var x = _context.Tipousuarios.FirstOrDefault(tu => tu.Id == entity.Id);
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1;
            }

            return false;
        }

        private static Tipousuario SetEntity(TipoUsuarioModel model, Tipousuario entity)
        {
            entity.Id = model.Id;
            entity.Descricao = model.Descricao;

            return entity;
        }
    }
}