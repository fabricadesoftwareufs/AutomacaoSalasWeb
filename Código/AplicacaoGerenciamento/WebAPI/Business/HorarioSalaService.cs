using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class HorarioSalaService : IHorarioSalaService
    {
        private readonly STR_DBContext _context;
        public HorarioSalaService(STR_DBContext context)
        {
            _context = context;
        }
        public List<HorarioSalaModel> GetAll()
            => _context.Horariosala
                .Select(hs => new HorarioSalaModel
                {
                    Id = hs.Id,
                    Data = hs.Data,
                    SalaId = hs.Sala,
                    HorarioInicio = hs.HorarioInicio,
                    HorarioFim = hs.HorarioFim,
                    Situacao = hs.Situacao,
                    Objetivo = hs.Objetivo,
                    UsuarioId = hs.Usuario

                }).ToList();
        public int Id { get; set; }

        public HorarioSalaModel GetById(int id)
            => _context.Horariosala
                .Where(hs => hs.Id == id)
                .Select(hs => new HorarioSalaModel
                {
                    Id = hs.Id,
                    Data = hs.Data,
                    SalaId = hs.Sala,
                    HorarioInicio = hs.HorarioInicio,
                    HorarioFim = hs.HorarioFim,
                    Situacao = hs.Situacao,
                    Objetivo = hs.Objetivo,
                    UsuarioId = hs.Usuario
                }).FirstOrDefault();

        public List<HorarioSalaModel> GetByIdSala(int id)
           => _context.Horariosala
               .Where(hs => hs.Sala == id)
               .Select(hs => new HorarioSalaModel
               {
                   Id = hs.Id,
                   Data = hs.Data,
                   SalaId = hs.Sala,
                   HorarioInicio = hs.HorarioInicio,
                   HorarioFim = hs.HorarioFim,
                   Situacao = hs.Situacao,
                   Objetivo = hs.Objetivo,
                   UsuarioId = hs.Usuario
               }).ToList();

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
            entity.Sala = model.SalaId;
            entity.HorarioInicio = model.HorarioInicio;
            entity.HorarioFim = model.HorarioFim;
            entity.Situacao = model.Situacao;
            entity.Objetivo = model.Objetivo;
            entity.Usuario = model.UsuarioId;


            return entity;
        }
    }
}
