using Model;
using Model.AuxModel;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class PlanejamentoService : IPlanejamentoService
    {
        private readonly STR_DBContext _context;
        public PlanejamentoService(STR_DBContext context)
        {
            _context = context;
        }

        public List<PlanejamentoModel> GetAll()
            => _context.Planejamento
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.Usuario,
                    SalaId = pl.Sala

                }).ToList();

        public PlanejamentoModel GetById(int id)
            => _context.Planejamento
                .Where(pl => pl.Id == id)
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.Usuario,
                    SalaId = pl.Sala
                }).FirstOrDefault();

        public List<PlanejamentoModel> GetByIdSala(int id)
            => _context.Planejamento
                .Where(pl => pl.Sala == id)
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.Usuario,
                    SalaId = pl.Sala
                }).ToList();


        public bool InsertListHorariosPlanjamento(PlanejamentoAuxModel entity)
        {
            using (var transcaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity.Horarios.Count == 0)
                        throw new ServiceException("Adicione pelo menos um horário!");

                    foreach (var horario in entity.Horarios)
                    {
                        if (TimeSpan.Compare(horario.HorarioFim, horario.HorarioInicio) != 1)
                            throw new ServiceException("Os horários possuem inconsistências, corrija e tente novamente");
                        else
                        {
                            Insert(new PlanejamentoModel
                            {
                                Id = entity.Planejamento.Id,
                                Objetivo = entity.Planejamento.Objetivo,
                                DataInicio = entity.Planejamento.DataInicio,
                                DataFim = entity.Planejamento.DataFim,
                                HorarioFim = horario.HorarioFim,
                                HorarioInicio = horario.HorarioInicio,
                                DiaSemana = horario.DiaSemana,
                                UsuarioId = entity.Planejamento.UsuarioId,
                                SalaId = entity.Planejamento.SalaId
                            });
                        }
                    }

                    InsertReservasPlanejamento(entity);
                    transcaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    transcaction.Rollback();
                    throw e;
                }
            }
        }

        private bool InsertReservasPlanejamento(PlanejamentoAuxModel entity)
        {
            try
            {
                var _horarioSalaService = new HorarioSalaService(_context);
                var dataCorrente = entity.Planejamento.DataInicio;
                var listaReservas = new List<HorarioSalaModel>();
                var addDays = 1;

                foreach (var item in entity.Horarios)
                {
                    while (dataCorrente >= entity.Planejamento.DataInicio && dataCorrente <= entity.Planejamento.DataFim)
                    {
                        if (((int)dataCorrente.DayOfWeek) == PlanejamentoViewModel.GetCodigoDia(item.DiaSemana))
                        {
                            _horarioSalaService.Insert(
                                new HorarioSalaModel
                                {
                                    HorarioFim = item.HorarioFim,
                                    HorarioInicio = item.HorarioInicio,
                                    SalaId = entity.Planejamento.SalaId,
                                    UsuarioId = entity.Planejamento.UsuarioId,
                                    Objetivo = entity.Planejamento.Objetivo,
                                    Situacao = "PENDENTE",
                                    Data = dataCorrente
                                });

                            addDays = 7;
                        }

                        dataCorrente = dataCorrente.AddDays(addDays);
                    }

                    dataCorrente = entity.Planejamento.DataInicio;
                    addDays = 1;
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Insert(PlanejamentoModel entity)
        {
            try
            {
                if (!(DateTime.Compare(entity.DataFim, entity.DataInicio) > 0) || TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) != 1)
                    throw new ServiceException("Sua Datas ou Horarios possuem inconsistências, corrija e tente novamente.");

                _context.Add(SetEntity(entity, new Planejamento()));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Remove(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var x = _context.Planejamento.Where(th => th.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        var save = _context.SaveChanges() == 1 ? true : false;
                        transaction.Commit();
                        return save;
                    }
                    else
                    {
                        throw new ServiceException("Algo deu errado, tente novamente em alguns minutos.");
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;

                }
            }
        }

        public bool Update(PlanejamentoModel entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!(DateTime.Compare(entity.DataFim, entity.DataInicio) > 0 && TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) == 1))
                        throw new ServiceException("Suas Datas/Horarios possuem inconsistências, corrija e tente novamente");

                    _context.Update(SetEntity(entity, new Planejamento()));
                    var save = _context.SaveChanges() == 1 ? true : false;
                    transaction.Commit();
                    return save;

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        private static Planejamento SetEntity(PlanejamentoModel model, Planejamento entity)
        {
            entity.Id = model.Id;
            entity.DataInicio = model.DataInicio;
            entity.DataFim = model.DataFim;
            entity.HorarioInicio = model.HorarioInicio;
            entity.HorarioFim = model.HorarioFim;
            entity.DiaSemana = model.DiaSemana;
            entity.Objetivo = model.Objetivo;
            entity.Sala = model.SalaId;
            entity.Usuario = model.UsuarioId;


            return entity;
        }

        public List<PlanejamentoModel> GetByIdUsuario(int idUsuario)
         => _context.Planejamento
                .Where(pl => pl.Usuario == idUsuario)
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.Usuario,
                    SalaId = pl.Sala
                }).ToList();


        public List<PlanejamentoModel> GetByIdOrganizacao(int idOrganizacao)
        {
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var query = (from pl in GetAll()
                         join sl in _salaService.GetAll() on pl.SalaId equals sl.Id
                         join bl in _blocoService.GetByIdOrganizacao(idOrganizacao) on sl.BlocoId equals bl.Id
                         select new PlanejamentoModel
                         {
                             Id = pl.Id,
                             DataFim = pl.DataFim,
                             DataInicio = pl.DataInicio,
                             DiaSemana = pl.DiaSemana,
                             HorarioFim = pl.HorarioFim,
                             HorarioInicio = pl.HorarioInicio,
                             SalaId = pl.SalaId,
                             UsuarioId = pl.UsuarioId
                         }).ToList();

            return query;
        }

        public bool RemoveByUsuario(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var x = _context.Planejamento.Where(th => th.Usuario == id);
                    if (x != null)
                    {
                        _context.RemoveRange(x);
                        var save = _context.SaveChanges() == 1 ? true : false;
                        transaction.Commit();
                        return save;
                    }
                    else
                    {
                        throw new ServiceException("Algo deu errado, tente novamente em alguns minutos.");
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;

                }
            }
        }
    }
}
