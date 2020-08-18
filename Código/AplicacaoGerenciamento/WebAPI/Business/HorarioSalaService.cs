using Model;
using Model.ViewModel;
using Org.BouncyCastle.Asn1.Cms;
using Persistence;
using Service.Interface;
using System;
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

        public List<HorarioSalaModel> GetByIdUsuario(int idUsuario)
        => _context.Horariosala
            .Where(hs => hs.Usuario == idUsuario)
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

        public List<HorarioSalaModel> GetByIdUsuarioAndDiaSemana(int idUsuario, string diaSemana)
        => _context.Horariosala
            .Where(hs => hs.Usuario == idUsuario && ((int)hs.Data.DayOfWeek) == PlanejamentoViewModel.GetCodigoDia(diaSemana.ToUpper()))
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

        public List<HorarioSalaModel> GetProximasReservasByIdUsuarioAndDiaSemana(int idUsuario, string diaSemana)
         => _context.Horariosala
             .Where(hs => hs.Usuario == idUsuario && ((int)hs.Data.DayOfWeek) == PlanejamentoViewModel.GetCodigoDia(diaSemana.ToUpper()) &&
                    hs.Data >= DateTime.Now.Date && hs.Data <= DateTime.Now.AddDays(6) && !hs.Situacao.Equals("CANCELADA"))
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

        public bool ConcelarReserva(int idReserva)
        {
            try
            {
                var reserva = GetById(idReserva);
                reserva.Situacao = "CANCELADA";

                return Update(reserva);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HorarioSalaModel VerificaSalaOcupada(int idSala, DateTime data, TimeSpan horarioInicio, TimeSpan horarioFim)
            => _context.Horariosala
               .Where(hs => hs.Sala == idSala && hs.Data == data && ((hs.HorarioInicio <= horarioInicio && horarioInicio <= hs.HorarioFim) || (hs.HorarioInicio <= horarioFim && horarioFim <= hs.HorarioFim)))
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

        public bool Insert(HorarioSalaModel entity)
        {
            try
            {
                if (VerificaSalaOcupada(entity.SalaId, entity.Data, entity.HorarioInicio, entity.HorarioFim) != null)
                    throw new ServiceException("Essa sala já possui reserva nessa data e horários, por favor, tente outra data ou horário!  ");

                if (TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) != 1)
                    throw new ServiceException("Os horários possuem inconsistências, corrija-os e tente novamente!");

                _context.Add(SetEntity(entity, new Horariosala()));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
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
