using Model;
using Model.ViewModel;
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
                    UsuarioId = hs.Usuario,
                    Planejamento = hs.Planejamento

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
                    UsuarioId = hs.Usuario,
                    Planejamento = hs.Planejamento
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
                   UsuarioId = hs.Usuario,
                   Planejamento = hs.Planejamento
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
                UsuarioId = hs.Usuario,
                Planejamento = hs.Planejamento
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
                UsuarioId = hs.Usuario,
                Planejamento = hs.Planejamento
            }).ToList();

        public List<HorarioSalaModel> GetReservasDaSemanaByIdSala(int idSala)
        {
            /*
             * Pegando a data atual e a data do próximo domingo
             */
            DateTime dataAtual = DateTime.Now;
            DateTime proximoDomingo;

            int nDia = (int)dataAtual.DayOfWeek;
 
            if (nDia == 0) proximoDomingo = dataAtual;
            else proximoDomingo = DateTime.Now.AddDays(7 - nDia).Date;

            return _context.Horariosala
             .Where(hs => hs.Sala == idSala && hs.Data.Date >= dataAtual.Date && hs.Data.Date <= proximoDomingo.Date && !hs.Situacao.Equals(HorarioSalaModel.SITUACAO_CANCELADA))
             .Select(hs => new HorarioSalaModel
             {
                 Id = hs.Id,
                 Data = hs.Data,
                 SalaId = hs.Sala,
                 HorarioInicio = hs.HorarioInicio,
                 HorarioFim = hs.HorarioFim,
                 Situacao = hs.Situacao,
                 Objetivo = hs.Objetivo,
                 UsuarioId = hs.Usuario,
                 Planejamento = hs.Planejamento
             }).ToList();
        }

        public List<HorarioSalaModel> GetProximasReservasByIdUsuarioAndDiaSemana(int idUsuario, string diaSemana)
         => _context.Horariosala
             .Where(hs => hs.Usuario == idUsuario && ((int)hs.Data.DayOfWeek) == PlanejamentoViewModel.GetCodigoDia(diaSemana.ToUpper()) &&
                    hs.Data >= DateTime.Now.Date && hs.Data <= DateTime.Now.AddDays(6) && !hs.Situacao.Equals(HorarioSalaModel.SITUACAO_CANCELADA))
             .Select(hs => new HorarioSalaModel
             {
                 Id = hs.Id,
                 Data = hs.Data,
                 SalaId = hs.Sala,
                 HorarioInicio = hs.HorarioInicio,
                 HorarioFim = hs.HorarioFim,
                 Situacao = hs.Situacao,
                 Objetivo = hs.Objetivo,
                 UsuarioId = hs.Usuario,
                 Planejamento = hs.Planejamento
             }).ToList();

        public bool ConcelarReserva(int idReserva)
        {
            try
            {
                var reserva = GetById(idReserva);
                reserva.Situacao = HorarioSalaModel.SITUACAO_CANCELADA;

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
                   UsuarioId = hs.Usuario,
                   Planejamento = hs.Planejamento
               }).FirstOrDefault();


        public bool VerificaSeEstaEmHorarioAula(int idUsuario, int idSala)
        {
            var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            var horaAtual = new TimeSpan(date.TimeOfDay.Hours, date.TimeOfDay.Minutes, date.TimeOfDay.Seconds);

            var query = _context.Horariosala
                        .Where(hs => hs.Sala == idSala && hs.Usuario == idUsuario && (horaAtual >= hs.HorarioInicio && horaAtual <= hs.HorarioFim) && date.Date == hs.Data)
                        .Select(hs => new HorarioSalaModel
                        {
                            Id = hs.Id,
                        }).FirstOrDefault();

            return query == null;
        }


        public bool Insert(HorarioSalaModel entity)
        {
            try
            {
                if (VerificaSalaOcupada(entity.SalaId, entity.Data, entity.HorarioInicio, entity.HorarioFim) != null)
                    throw new ServiceException("Essa sala já possui reserva nessa data e horários, por favor, tente outra data ou horário!  ");

                if (TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) != 1)
                    throw new ServiceException("Os horários possuem inconsistências, corrija-os e tente novamente!");

                _context.Add(SetEntity(entity, new Horariosala()));
                return _context.SaveChanges() == 1;
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
                return _context.SaveChanges() == 1;
            }

            return false;
        }

        public bool RemoveByIdPlanejamento(int idPlanejamento)
        {
            try
            {
                var x = _context.Horariosala.Where(th => th.Planejamento == idPlanejamento).ToList();
                if (x != null)
                {
                    _context.RemoveRange(x);
                    return _context.SaveChanges() == 1;
                }
            }
            catch (Exception e)
            {
                throw new ServiceException("Houve um problema ao remover reservas associadas ao planejamento, por favor tente novamente mais tarde!");
            }

            return false;
        }

        public bool Update(HorarioSalaModel entity)
        {
            var x = _context.Horariosala.Where(th => th.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1;
            }

            return false;
        }
        public bool UpdateColumnPlanejamentoForNull(int idPlanejamento)
        {
            try
            {
                var x = _context.Horariosala.Where(th => th.Planejamento == idPlanejamento).ToList();
                if (x != null)
                {
                    x.ForEach(r => r.Planejamento = null);
                    _context.UpdateRange(x);
                    return _context.SaveChanges() == 1;
                }
            }
            catch (Exception e)
            {
                throw new ServiceException("Houve um problema ao atualizar reservas associadas ao planejamento, por favor tente novamente mais tarde!");
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
            entity.Planejamento = model.Planejamento;


            return entity;
        }

        public bool RemoveByUsuario(int id)
        {
            var x = _context.Horariosala.Where(th => th.Id == id);
            if (x != null)
            {
                _context.RemoveRange(x);
                return _context.SaveChanges() == 1;
            }

            return false;
        }
    }
}
