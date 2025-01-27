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
        private readonly SalasDBContext _context;

        public HorarioSalaService(SalasDBContext context)
        {
            _context = context;
        }

        public List<HorarioSalaModel> GetAll()
            => _context.Horariosalas
                .Select(hs => new HorarioSalaModel
                {
                    Id = hs.Id,
                    Data = hs.Data,
                    SalaId = hs.IdSala,
                    HorarioInicio = hs.HorarioInicio,
                    HorarioFim = hs.HorarioFim,
                    Situacao = hs.Situacao,
                    Objetivo = hs.Objetivo,
                    UsuarioId = hs.IdUsuario,
                    Planejamento = hs.IdPlanejamento

                }).ToList();
        public int Id { get; set; }

        public HorarioSalaModel GetById(uint id)
            => _context.Horariosalas
                .Where(hs => hs.Id == id)
                .Select(hs => new HorarioSalaModel
                {
                    Id = hs.Id,
                    Data = hs.Data,
                    SalaId = hs.IdSala,
                    HorarioInicio = hs.HorarioInicio,
                    HorarioFim = hs.HorarioFim,
                    Situacao = hs.Situacao,
                    Objetivo = hs.Objetivo,
                    UsuarioId = hs.IdUsuario,
                    Planejamento = hs.IdPlanejamento
                }).FirstOrDefault();

        public List<HorarioSalaModel> GetByIdSala(uint id)
           => _context.Horariosalas
               .Where(hs => hs.IdSala == id)
               .Select(hs => new HorarioSalaModel
               {
                   Id = hs.Id,
                   Data = hs.Data,
                   SalaId = hs.IdSala,
                   HorarioInicio = hs.HorarioInicio,
                   HorarioFim = hs.HorarioFim,
                   Situacao = hs.Situacao,
                   Objetivo = hs.Objetivo,
                   UsuarioId = hs.IdUsuario,
                   Planejamento = hs.IdPlanejamento
               }).ToList();

        public List<HorarioSalaModel> GetByIdUsuario(uint idUsuario)
        => _context.Horariosalas
            .Where(hs => hs.IdUsuario == idUsuario)
            .Select(hs => new HorarioSalaModel
            {
                Id = hs.Id,
                Data = hs.Data,
                SalaId = hs.IdSala,
                HorarioInicio = hs.HorarioInicio,
                HorarioFim = hs.HorarioFim,
                Situacao = hs.Situacao,
                Objetivo = hs.Objetivo,
                UsuarioId = hs.IdUsuario,
                Planejamento = hs.IdPlanejamento
            }).ToList();

        public List<HorarioSalaModel> GetByIdUsuarioAndDiaSemana(uint idUsuario, string diaSemana)
        => _context.Horariosalas
            .Where(hs => hs.IdUsuario == idUsuario && ((int)hs.Data.DayOfWeek) == PlanejamentoViewModel.GetCodigoDia(diaSemana.ToUpper()))
            .Select(hs => new HorarioSalaModel
            {
                Id = hs.Id,
                Data = hs.Data,
                SalaId = hs.IdSala,
                HorarioInicio = hs.HorarioInicio,
                HorarioFim = hs.HorarioFim,
                Situacao = hs.Situacao,
                Objetivo = hs.Objetivo,
                UsuarioId = hs.IdUsuario,
                Planejamento = hs.IdPlanejamento
            }).ToList();

        public List<HorarioSalaModel> GetReservasDaSemanaByIdSala(uint idSala)
        {
            /*
             * Pegando a data atual e a data do próximo domingo
             */
            DateTime dataAtual = DateTime.Now;
            DateTime proximoDomingo;

            int nDia = (int)dataAtual.DayOfWeek;
            if (nDia == 0) proximoDomingo = dataAtual;
            else proximoDomingo = DateTime.Now.AddDays(7 - nDia).Date;

            return _context.Horariosalas
             .Where(hs => hs.IdSala == idSala && hs.Data.Date >= dataAtual.Date && hs.Data.Date <= proximoDomingo.Date && !hs.Situacao.Equals(HorarioSalaModel.SITUACAO_CANCELADA))
             .Select(hs => new HorarioSalaModel
             {
                 Id = hs.Id,
                 Data = hs.Data,
                 SalaId = hs.IdSala,
                 HorarioInicio = hs.HorarioInicio,
                 HorarioFim = hs.HorarioFim,
                 Situacao = hs.Situacao,
                 Objetivo = hs.Objetivo,
                 UsuarioId = hs.IdUsuario,
                 Planejamento = hs.IdPlanejamento
             }).ToList();
        }

        public List<HorarioSalaModel> GetReservasDeHojeByIdSala(uint idSala)
        {
            return _context.Horariosalas
             .Where(hs => hs.IdSala == idSala && hs.Data.Date == DateTime.Now.Date && !hs.Situacao.Equals(HorarioSalaModel.SITUACAO_CANCELADA))
             .Select(hs => new HorarioSalaModel
             {
                 Id = hs.Id,
                 Data = hs.Data,
                 SalaId = hs.IdSala,
                 HorarioInicio = hs.HorarioInicio,
                 HorarioFim = hs.HorarioFim,
                 Situacao = hs.Situacao,
                 Objetivo = hs.Objetivo,
                 UsuarioId = hs.IdUsuario,
                 Planejamento = hs.IdPlanejamento
             }).ToList();
        }

        public List<HorarioSalaModel> GetReservasDeHojeByUuid(string uuid)
        {

            var lista =  _context.Horariosalas
             .Join(_context.Hardwaredesalas,
              horario => horario.IdSala,
              hard => hard.IdSala,
              (horario, hard) => new {Horario = horario, Hardware = hard})
             .Where(hs => hs.Horario.Data.Date == DateTime.Now.Date 
                      && !hs.Horario.Situacao.Equals(HorarioSalaModel.SITUACAO_CANCELADA)
                      && !string.IsNullOrWhiteSpace(hs.Hardware.Uuid) 
                      && hs.Hardware.Uuid.Trim().Equals(uuid.Trim()))
             .Select(hs => new HorarioSalaModel
                 {
                     Id = hs.Horario.Id,
                     Data = hs.Horario.Data,
                     SalaId = hs.Horario.IdSala,
                     HorarioInicio = hs.Horario.HorarioInicio,
                     HorarioFim = hs.Horario.HorarioFim,
                     Situacao = hs.Horario.Situacao,
                     Objetivo = hs.Horario.Objetivo,
                     UsuarioId = hs.Horario.IdUsuario,
                     Planejamento = hs.Horario.IdPlanejamento
                 }).ToList();

            return lista;
        }

        public IEnumerable<HorarioSalaModel> GetProximasReservasByIdUsuarioAndDiaSemana(uint idUsuario, string diaSemana)
        {
            var reservas = _context.Horariosalas
               .Where(hs => hs.IdUsuario == idUsuario)
               .Select(hs => new HorarioSalaModel
               {
                   Id = hs.Id,
                   Data = hs.Data,
                   SalaId = hs.IdSala,
                   HorarioInicio = hs.HorarioInicio,
                   HorarioFim = hs.HorarioFim,
                   Situacao = hs.Situacao,
                   Objetivo = hs.Objetivo,
                   UsuarioId = hs.IdUsuario,
                   Planejamento = hs.IdPlanejamento
               }).ToList();

            return reservas.Where(hs => hs.Data >= DateTime.Now.Date && ((int)hs.Data.DayOfWeek) == PlanejamentoViewModel.GetCodigoDia(diaSemana.ToUpper())
              && !hs.Situacao.Equals(HorarioSalaModel.SITUACAO_CANCELADA));
        }

        public bool ConcelarReserva(uint idReserva)
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

        public HorarioSalaModel VerificaSalaOcupada(uint idSala, DateTime data, TimeSpan horarioInicio, TimeSpan horarioFim)
            => _context.Horariosalas
               .Where(hs => hs.IdSala == idSala && hs.Data == data && ((hs.HorarioInicio <= horarioInicio && horarioInicio <= hs.HorarioFim) || (hs.HorarioInicio <= horarioFim && horarioFim <= hs.HorarioFim)))
               .Select(hs => new HorarioSalaModel
               {
                   Id = hs.Id,
                   Data = hs.Data,
                   SalaId = hs.IdSala,
                   HorarioInicio = hs.HorarioInicio,
                   HorarioFim = hs.HorarioFim,
                   Situacao = hs.Situacao,
                   Objetivo = hs.Objetivo,
                   UsuarioId = hs.IdUsuario,
                   Planejamento = hs.IdPlanejamento
               }).FirstOrDefault();


        public bool VerificaSeEstaEmHorarioAula(uint idUsuario, uint idSala)
        {
            var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            var horaAtual = new TimeSpan(date.TimeOfDay.Hours, date.TimeOfDay.Minutes, date.TimeOfDay.Seconds);


            var query = _context.Horariosalas
                        .Where(hs => hs.IdSala == idSala && hs.IdUsuario == idUsuario && (horaAtual >= hs.HorarioInicio && horaAtual <= hs.HorarioFim) && date.Date == hs.Data)
                        .Select(hs => new HorarioSalaModel
                        {
                            Id = hs.Id,
                        }).FirstOrDefault();

            return query != null;
        }


        public bool Insert(HorarioSalaModel entity)
        {
            try
            {
                if (VerificaSalaOcupada(entity.SalaId, entity.Data, entity.HorarioInicio, entity.HorarioFim) != null)
                    throw new ServiceException("Esta sala já possui uma reserva para a data e horários selecionados. Por favor, escolha outra data ou horário.");

                if (TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) != 1)
                    throw new ServiceException("Os horários possuem inconsistências, corrija-os e tente novamente.");

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
            var x = _context.Horariosalas.Where(th => th.Id == id).FirstOrDefault();
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
                var x = _context.Horariosalas.Where(th => th.IdPlanejamento == idPlanejamento).ToList();
                if (x != null)
                {
                    _context.RemoveRange(x);
                    return _context.SaveChanges() == 1;
                }
            }
            catch (Exception e)
            {
                throw new ServiceException("Houve um problema ao remover reservas associadas ao planejamento, por favor tente novamente mais tarde.");
            }

            return false;
        }

        public bool Update(HorarioSalaModel entity)
        {
            var x = _context.Horariosalas.Where(th => th.Id == entity.Id).FirstOrDefault();
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
                var x = _context.Horariosalas.Where(th => th.IdPlanejamento == idPlanejamento).ToList();
                if (x != null)
                {
                    x.ForEach(r => r.IdPlanejamento = null);
                    _context.UpdateRange(x);
                    return _context.SaveChanges() == 1;
                }
            }
            catch (Exception e)
            {
                throw new ServiceException("Houve um problema ao atualizar reservas associadas ao planejamento, por favor tente novamente mais tarde.");
            }

            return false;
        }

        private static Horariosala SetEntity(HorarioSalaModel model, Horariosala entity)
        {
            entity.Id = model.Id;
            entity.Data = model.Data;
            entity.IdSala = model.SalaId;
            entity.HorarioInicio = model.HorarioInicio;
            entity.HorarioFim = model.HorarioFim;
            entity.Situacao = model.Situacao;
            entity.Objetivo = model.Objetivo;
            entity.IdUsuario = model.UsuarioId;
            entity.IdPlanejamento = model.Planejamento;


            return entity;
        }

        public bool RemoveByUsuario(uint id)
        {
            var x = _context.Horariosalas.Where(th => th.Id == id);
            if (x != null)
            {
                _context.RemoveRange(x);
                return _context.SaveChanges() == 1;
            }

            return false;
        }
    }
}
