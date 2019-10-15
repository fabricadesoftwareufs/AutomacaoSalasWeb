using Models;
using Persistences;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class DisciplinaService : IService<DisciplinaModel>
    {
        private readonly ContextDb _context;
        public DisciplinaService(ContextDb context)
        {
            _context = context;
        }
        public List<DisciplinaModel> GetAll() => _context.Disciplina.Select(d => new DisciplinaModel { Id = d.Id, Codigo = d.Codigo, Nome = d.Nome }).ToList();

        public DisciplinaModel GetById(int id) => _context.Disciplina.Where(d => d.Id == id).Select(d => new DisciplinaModel { Id = d.Id, Codigo = d.Codigo, Nome = d.Nome }).FirstOrDefault();

        public bool Insert(DisciplinaModel entity)
        {
            _context.Add(SetEntity(entity, new Disciplina()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var disc = _context.Disciplina.Where(d => d.Id == id).FirstOrDefault();
            if (disc != null)
            {
                _context.Remove(disc);
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        public bool Update(DisciplinaModel entity)
        {
            var disc = _context.Disciplina.Where(d => d.Id == entity.Id).FirstOrDefault();
            if (disc != null)
            {
                _context.Update(SetEntity(entity, disc));
                return _context.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        private static Disciplina SetEntity(DisciplinaModel model, Disciplina entity)
        {
            entity.Id = model.Id;
            entity.Nome = model.Nome;
            entity.Codigo = model.Codigo;

            return entity;
        }
    }
}
