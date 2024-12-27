﻿using Model.AuxModel;
using Model.ViewModel;
using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Service
{
    public class PlanejamentoService : IPlanejamentoService
    {
        private readonly SalasDBContext _context;

        public PlanejamentoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista de todos os planejamentos cadastrados.
        /// </summary>
        public List<PlanejamentoModel> GetAll()
            => _context.Planejamentos
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

        /// <summary>
        /// Retorna um planejamento específico pelo seu ID.
        /// </summary>
        public PlanejamentoModel GetById(int id)
            => _context.Planejamentos
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

        /// <summary>
        /// Retorna uma lista de planejamentos de uma sala específica.
        /// </summary>
        public List<PlanejamentoModel> GetByIdSala(uint id)
            => _context.Planejamentos
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

        /// <summary>
        /// Retorna uma lista de planejamentos filtrados pela organização.
        /// </summary>
        public List<PlanejamentoModel> GetByIdOrganizacao(uint idOrganizacao)
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
                             UsuarioId = pl.UsuarioId,
                             BlocoNome = bl.Titulo
                         }).ToList();

            return query;
        }

        /// <summary>
        /// Insere um planejamento com uma lista de horários associados.
        /// </summary>
        public bool InsertPlanejamentoWithListHorarios(PlanejamentoAuxModel model)
        {
            try
            {
                ValidaListaHorariosPlanejamento(model.Horarios);

                foreach (var horario in model.Horarios)
                {
                    Insert(new PlanejamentoModel
                    {
                        Id = model.Planejamento.Id,
                        Objetivo = model.Planejamento.Objetivo,
                        DataInicio = model.Planejamento.DataInicio,
                        DataFim = model.Planejamento.DataFim,
                        HorarioFim = horario.HorarioFim,
                        HorarioInicio = horario.HorarioInicio,
                        DiaSemana = horario.DiaSemana,
                        UsuarioId = model.Planejamento.UsuarioId,
                        SalaId = model.Planejamento.SalaId
                    });
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Insere um novo planejamento.
        /// </summary>
        public bool Insert(PlanejamentoModel planejamentoModel)
        {
            using (var transcaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!(DateTime.Compare(planejamentoModel.DataFim, planejamentoModel.DataInicio) > 0) || TimeSpan.Compare(planejamentoModel.HorarioFim, planejamentoModel.HorarioInicio) != 1)
                        throw new ServiceException("Sua Datas ou Horarios possuem inconsistências, corrija-os e tente novamente.");

                    var planejamentoInserido = new Planejamento();
                    _context.Add(SetEntity(planejamentoModel, planejamentoInserido));
                    var save = _context.SaveChanges() == 1 ? true : false;

                    planejamentoModel.Id = planejamentoInserido.Id;

                    InsertReservasPlanejamento(planejamentoModel);

                    transcaction.Commit();
                    return save;
                }
                catch (Exception e)
                {
                    transcaction.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Remove um planejamento pelo seu ID.
        /// </summary>
        public bool Remove(int id, bool excluiReservas)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var _reservaContext = new HorarioSalaService(_context);

                    if (excluiReservas) _reservaContext.RemoveByIdPlanejamento(id);
                    else _reservaContext.UpdateColumnPlanejamentoForNull(id);

                    var x = _context.Planejamentos.Where(th => th.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        var save = _context.SaveChanges() == 1 ? true : false;
                        transaction.Commit();
                        return save;
                    }
                    else throw new ServiceException("Algo deu errado, tente novamente em alguns minutos.");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Atualiza os dados de um planejamento existente.
        /// </summary>
        public bool Update(PlanejamentoModel entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!(DateTime.Compare(entity.DataFim, entity.DataInicio) > 0 && TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) == 1))
                        throw new ServiceException("Suas Datas/Horarios possuem inconsistências, corrija-os e tente novamente");

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

        /// <summary>
        /// Retorna uma lista de planejamentos filtrados pelo ID do usuário.
        /// </summary>
        public List<PlanejamentoModel> GetByIdUsuario(uint idUsuario)
         => _context.Planejamentos
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

        /// <summary>
        /// Remove todos os planejamentos de um usuário específico.
        /// </summary>
        public bool RemoveByUsuario(uint id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var x = _context.Planejamentos.Where(th => th.Usuario == id);
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

        /// <summary>
        /// Converte um modelo de PlanejamentoModel para a entidade Planejamento.
        /// </summary>
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

        /// <summary>
        /// Valida uma lista de horários associados a um planejamento.
        /// </summary>
        private void ValidaListaHorariosPlanejamento(List<HorarioPlanejamentoAuxModel> horarios)
        {
            try
            {
                if (horarios.Count == 0)
                    throw new ServiceException("Adicione pelo menos um horário!");

                foreach (var horario in horarios)
                    if (TimeSpan.Compare(horario.HorarioFim, horario.HorarioInicio) != 1)
                        throw new ServiceException("Os horários possuem inconsistências, corrija-os e tente novamente");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Insere reservas de horário relacionadas ao planejamento.
        /// </summary>
        private bool InsertReservasPlanejamento(PlanejamentoModel planejamento)
        {
            try
            {
                var _horarioSalaService = new HorarioSalaService(_context);
                var dataCorrente = planejamento.DataInicio;
                var addDays = 1;

                while (dataCorrente >= planejamento.DataInicio && dataCorrente <= planejamento.DataFim)
                {
                    if (((int)dataCorrente.DayOfWeek) == PlanejamentoViewModel.GetCodigoDia(planejamento.DiaSemana))
                    {
                        _horarioSalaService.Insert(
                              new HorarioSalaModel
                              {
                                  HorarioFim = planejamento.HorarioFim,
                                  HorarioInicio = planejamento.HorarioInicio,
                                  SalaId = planejamento.SalaId,
                                  UsuarioId = planejamento.UsuarioId,
                                  Objetivo = planejamento.Objetivo,
                                  Planejamento = planejamento.Id,
                                  Situacao = HorarioSalaModel.SITUACAO_APROVADA,
                                  Data = dataCorrente
                              });

                        addDays = 7;
                    }

                    dataCorrente = dataCorrente.AddDays(addDays);
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
