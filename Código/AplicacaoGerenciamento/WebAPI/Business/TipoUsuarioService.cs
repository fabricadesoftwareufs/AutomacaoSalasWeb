using Model;
using Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class TipoUsuarioService : IService<TipoUsuarioModel>
    {
        private readonly STR_DBContext _context;
        public TipoUsuarioService(STR_DBContext context)
        {
            _context = context;
        }
        public List<TipoUsuarioModel> GetAll() => _context.Tipousuario.Select(tu => new TipoUsuarioModel { Id = tu.Id, Descricao = tu.Descricao }).ToList();

        public TipoUsuarioModel GetById(int id) => _context.Tipousuario.Where(tu => tu.Id == id).Select(tu => new TipoUsuarioModel { Id = tu.Id, Descricao = tu.Descricao }).FirstOrDefault();

        public bool Insert(TipoUsuarioModel entity)
        {
            _context.Add(SetEntity(entity, new Tipousuario()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Tipousuario.Where(tu => tu.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(TipoUsuarioModel entity)
        {
            var x = _context.Tipousuario.Where(tu => tu.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Tipousuario SetEntity(TipoUsuarioModel model, Tipousuario entity)
        {
            entity.Id = model.Id;
            entity.Descricao = model.Descricao;

            return entity;
        }

        public List<TipoUsuarioModel> GetSelectedList()
         => _context.Tipousuario.Select(s => new TipoUsuarioModel { Id = s.Id, Descricao = string.Format("{0} - {1}", s.Id, s.Descricao) }).ToList();

    }
}
