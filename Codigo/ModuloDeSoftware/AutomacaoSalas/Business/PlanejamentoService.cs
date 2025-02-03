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
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas ao planejamento de salas.
    /// </summary>
    public class PlanejamentoService : IPlanejamentoService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Construtor do serviço de planejamento.
        /// </summary>
        /// <param name="context">Contexto do banco de dados para operações de persistência.</param>
        public PlanejamentoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recupera todos os planejamentos cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de planejamentos existentes.</returns>
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
                    UsuarioId = pl.IdUsuario,
                    SalaId = pl.IdSala

                }).ToList();

        /// <summary>
        /// Recupera um planejamento pelo seu identificador único.
        /// </summary>
        /// <param name="id">Identificador do planejamento.</param>
        /// <returns>Modelo do planejamento encontrado ou null se não existir.</returns>
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
                    UsuarioId = pl.IdUsuario,
                    SalaId = pl.IdSala
                }).FirstOrDefault();

        /// <summary>
        /// Recupera os planejamentos de uma sala específica.
        /// </summary>
        /// <param name="id">Identificador da sala.</param>
        /// <returns>Lista de planejamentos da sala especificada.</returns>
        public List<PlanejamentoModel> GetByIdSala(uint id)
            => _context.Planejamentos
                .Where(pl => pl.IdSala == id)
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.IdUsuario,
                    SalaId = pl.IdSala
                }).ToList();


        /// <summary>
        /// Recupera os planejamentos de uma organização específica.
        /// </summary>
        /// <param name="idOrganizacao">Identificador da organização.</param>
        /// <returns>Lista de planejamentos da organização especificada.</returns>
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
        /// Insere um planejamento com uma lista de horários.
        /// </summary>
        /// <param name="model">Modelo auxiliar contendo o planejamento e os horários associados.</param>
        /// <returns>True se a inserção for bem-sucedida, false caso contrário.</returns>
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
        /// Insere um novo planejamento no sistema.
        /// </summary>
        /// <param name="planejamentoModel">Modelo do planejamento a ser inserido.</param>
        /// <returns>True se a inserção for bem-sucedida, false caso contrário.</returns>
        /// <exception cref="ServiceException">Lançada se houver inconsistências nos horários ou datas.</exception>
        public bool Insert(PlanejamentoModel planejamentoModel)
        {
            using (var transcaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!(DateTime.Compare(planejamentoModel.DataFim, planejamentoModel.DataInicio) > 0) || TimeSpan.Compare(planejamentoModel.HorarioFim, planejamentoModel.HorarioInicio) != 1)
                        throw new ServiceException("As datas ou horários informados apresentam inconsistências. Por favor, corrija-os e tente novamente.");

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
        /// Remove um planejamento pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do planejamento.</param>
        /// <param name="excluiReservas">Indica se as reservas associadas devem ser excluídas.</param>
        /// <returns>True se a remoção for bem-sucedida, false caso contrário.</returns>
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
                    else throw new ServiceException("Ocorreu um erro. Por favor, tente novamente em alguns minutos.");
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
        /// <param name="entity">Modelo do planejamento com os dados atualizados.</param>
        /// <returns>True se a atualização for bem-sucedida, false caso contrário.</returns>
        public bool Update(PlanejamentoModel entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!(DateTime.Compare(entity.DataFim, entity.DataInicio) > 0 && TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) == 1))
                        throw new ServiceException("As suas datas ou horários apresentam inconsistências. Por favor, corrija-os e tente novamente.");

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
        /// Recupera os planejamentos de um usuário específico.
        /// </summary>
        /// <param name="idUsuario">Identificador do usuário.</param>
        /// <returns>Lista de planejamentos do usuário especificado.</returns>
        public List<PlanejamentoModel> GetByIdUsuario(uint idUsuario)
         => _context.Planejamentos
                .Where(pl => pl.IdUsuario == idUsuario)
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.IdUsuario,
                    SalaId = pl.IdSala
                }).ToList();

        /// <summary>
        /// Remove todos os planejamentos de um usuário específico.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <returns>True se a remoção for bem-sucedida, false caso contrário.</returns>
        public bool RemoveByUsuario(uint id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var x = _context.Planejamentos.Where(th => th.IdUsuario == id);
                    if (x != null)
                    {
                        _context.RemoveRange(x);
                        var save = _context.SaveChanges() == 1 ? true : false;
                        transaction.Commit();
                        return save;
                    }
                    else
                    {
                        throw new ServiceException("Ocorreu um erro. Por favor, tente novamente em alguns minutos.");
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
        /// Método auxiliar para mapear um modelo de planejamento para uma entidade de planejamento.
        /// </summary>
        /// <param name="model">Modelo de planejamento de origem.</param>
        /// <param name="entity">Entidade de planejamento de destino.</param>
        /// <returns>Entidade de planejamento atualizada.</returns>
        private static Planejamento SetEntity(PlanejamentoModel model, Planejamento entity)
        {
            entity.Id = model.Id;
            entity.DataInicio = model.DataInicio;
            entity.DataFim = model.DataFim;
            entity.HorarioInicio = model.HorarioInicio;
            entity.HorarioFim = model.HorarioFim;
            entity.DiaSemana = model.DiaSemana;
            entity.Objetivo = model.Objetivo;
            entity.IdSala = model.SalaId;
            entity.IdUsuario = model.UsuarioId;


            return entity;
        }

        /// <summary>
        /// Valida a lista de horários do planejamento, verificando se há pelo menos um horário e se os horários de início e fim são válidos.
        /// </summary>
        /// <param name="horarios">Lista de horários a serem validados.</param>
        /// <exception cref="ServiceException">Lançada se não houver horários ou se houver inconsistências nos horários.</exception>
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
        /// Insere reservas de horários para o planejamento, associando os horários aprovados às datas corretas dentro do intervalo de tempo do planejamento.
        /// </summary>
        /// <param name="planejamento">Modelo de planejamento contendo as informações de horário e sala.</param>
        /// <returns>True se as reservas forem inseridas com sucesso, false caso contrário.</returns>
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