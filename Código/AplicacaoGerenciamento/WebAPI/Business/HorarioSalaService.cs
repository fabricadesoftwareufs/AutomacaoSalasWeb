using Models;
using Persistences;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class HorarioSalaService : IService<HorarioSalaModel>
    {
        private readonly str_dbContext _context;
        public HorarioSalaService(str_dbContext context)
        {
            _context = context;
        }
        public List<HorarioSalaModel> GetAll()
            => _context.Horariosala
                .Select(hs => new HorarioSalaModel
                {
                    Id = hs.Id,
                    Data = hs.Data,
                    HoraInicio = hs.HoraInicio,
                    HoraFim = hs.HoraFim,
                    Turno = hs.Turno,
                    QtdAlunos = hs.QtdAlunos,
                    UsuarioId = hs.Usuario,
                    SalaId = hs.Sala,
                    DisciplinaId = (int)hs.Disciplina
                }).ToList();

        public HorarioSalaModel GetById(int id)
            => _context.Horariosala
                .Where(hs => hs.Id == id)
                .Select(hs => new HorarioSalaModel
                {
                    Id = hs.Id,
                    Data = hs.Data,
                    HoraInicio = hs.HoraInicio,
                    HoraFim = hs.HoraFim,
                    Turno = hs.Turno,
                    QtdAlunos = hs.QtdAlunos,
                    UsuarioId = hs.Usuario,
                    SalaId = hs.Sala,
                    DisciplinaId = (int)hs.Disciplina
                }).FirstOrDefault();

        public bool Insert(HorarioSalaModel entity)
        {
            _context.Add(SetEntity(entity, new Horariosala()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Horariosala.Where(th => th.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(HorarioSalaModel entity)
        {
            var x = _context.Horariosala.Where(th => th.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Horariosala SetEntity(HorarioSalaModel model, Horariosala entity)
        {
            entity.Id = model.Id;
            entity.Data = model.Data;
            entity.HoraInicio = model.HoraInicio;
            entity.HoraFim = model.HoraFim;
            entity.Turno = model.Turno;
            entity.QtdAlunos = model.QtdAlunos;
            entity.Usuario = model.UsuarioId;
            entity.Sala = model.SalaId;
            entity.Disciplina = model.DisciplinaId;

            return entity;
        }
    }
}
