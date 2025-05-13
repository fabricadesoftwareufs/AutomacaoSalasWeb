using Model;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Serviço responsável por gerenciar as operações relacionadas aos equipamentos.
    /// </summary>
    public class EquipamentoService : IEquipamentoService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Construtor da classe EquipamentoService.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public EquipamentoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém um equipamento pelo seu ID.
        /// </summary>
        /// <param name="idEquipamento">ID do equipamento.</param>
        /// <returns>Equipamento correspondente ou null se não encontrado.</returns>
        public EquipamentoModel GetByIdEquipamento(int idEquipamento)
        {
            var equipamento = _context.Equipamentos
                .Where(eq => eq.Id == idEquipamento)
                .Select(eq => new
                {
                    Equipamento = eq,
                    Modelo = _context.Modeloequipamentos.FirstOrDefault(m => m.Id == eq.IdModeloEquipamento)
                })
                .FirstOrDefault();

            if (equipamento == null)
                return null;

            var marca = equipamento.Modelo != null
                ? _context.Marcaequipamentos.FirstOrDefault(m => m.Id == equipamento.Modelo.IdMarcaEquipamento)
                : null;

            return new EquipamentoModel
            {
                Id = equipamento.Equipamento.Id,
                Modelo = equipamento.Modelo?.Nome,
                Marca = marca?.Nome,
                Descricao = equipamento.Equipamento.Descricao,
                Sala = equipamento.Equipamento.IdSala,
                TipoEquipamento = equipamento.Equipamento.TipoEquipamento,
                HardwareDeSala = equipamento.Equipamento.IdHardwareDeSala.HasValue ?
                    (uint)equipamento.Equipamento.IdHardwareDeSala : 0,
                IdModeloEquipamento = equipamento.Equipamento.IdModeloEquipamento
            };
        }


        /// <summary>
        /// Obtém um equipamento com base no ID da sala e tipo do equipamento.
        /// </summary>
        /// <param name="idSala">ID da sala.</param>
        /// <param name="tipo">Tipo do equipamento.</param>
        /// <returns>Equipamento correspondente ou null se não encontrado.</returns>
        public EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo)
    => _context.Equipamentos
           .Join(_context.Modeloequipamentos,
               eq => eq.IdModeloEquipamento,
               modelo => modelo.Id,
               (eq, modelo) => new { Equipamento = eq, Modelo = modelo })
           .Join(_context.Marcaequipamentos,
                combinado => combinado.Modelo.IdMarcaEquipamento,
                marca => marca.Id,
                (combinado, marca) => new { Combinado = combinado, Marca = marca })
           .Where(resultado => resultado.Combinado.Equipamento.IdSala == idSala &&
                  resultado.Combinado.Equipamento.TipoEquipamento.ToUpper().Equals(tipo.ToUpper()))
           .Select(resultado => new EquipamentoModel
           {
               Id = resultado.Combinado.Equipamento.Id,
               Modelo = resultado.Combinado.Modelo.Nome,
               Marca = resultado.Marca.Nome,
               Descricao = resultado.Combinado.Equipamento.Descricao,
               Sala = resultado.Combinado.Equipamento.IdSala,
               TipoEquipamento = resultado.Combinado.Equipamento.TipoEquipamento,
               HardwareDeSala = (uint)resultado.Combinado.Equipamento.IdHardwareDeSala
           }).FirstOrDefault();

        /// <summary>
        /// Obtém todos os equipamentos de uma sala.
        /// </summary>
        /// <param name="idSala">ID da sala.</param>
        /// <returns>Lista de equipamentos da sala.</returns>
        public List<EquipamentoModel> GetByIdSala(uint idSala)
    => _context.Equipamentos
           .Join(_context.Modeloequipamentos,
               eq => eq.IdModeloEquipamento,
               modelo => modelo.Id,
               (eq, modelo) => new { Equipamento = eq, Modelo = modelo })
           .Join(_context.Marcaequipamentos,
                combinado => combinado.Modelo.IdMarcaEquipamento,
                marca => marca.Id,
                (combinado, marca) => new { Combinado = combinado, Marca = marca })
           .Where(resultado => resultado.Combinado.Equipamento.IdSala == idSala)
           .Select(resultado => new EquipamentoModel
           {
               Id = resultado.Combinado.Equipamento.Id,
               Modelo = resultado.Combinado.Modelo.Nome,
               Marca = resultado.Marca.Nome,
               Descricao = resultado.Combinado.Equipamento.Descricao,
               Sala = resultado.Combinado.Equipamento.IdSala,
               TipoEquipamento = resultado.Combinado.Equipamento.TipoEquipamento,
               HardwareDeSala = (uint)resultado.Combinado.Equipamento.IdHardwareDeSala
           }).ToList();

        /// <summary>
        /// Obtém todos os equipamentos registrados.
        /// </summary>
        /// <returns>Lista de todos os equipamentos.</returns>
        /// <summary>
        /// Obtém todos os equipamentos registrados, incluindo aqueles sem modelo ou marca associados.
        /// </summary>
        /// <returns>Lista de todos os equipamentos.</returns>
        public List<EquipamentoModel> GetAll()
        {
            return _context.Equipamentos
                .Select(eq => new
                {
                    Equipamento = eq,
                    Modelo = _context.Modeloequipamentos.FirstOrDefault(m => m.Id == eq.IdModeloEquipamento)
                })
                .ToList()
                .Select(item =>
                {
                    var marca = item.Modelo != null
                        ? _context.Marcaequipamentos.FirstOrDefault(m => m.Id == item.Modelo.IdMarcaEquipamento)
                        : null;

                    return new EquipamentoModel
                    {
                        Id = item.Equipamento.Id,
                        Modelo = item.Modelo?.Nome,
                        Marca = marca?.Nome,
                        Descricao = item.Equipamento.Descricao,
                        Sala = item.Equipamento.IdSala,
                        TipoEquipamento = item.Equipamento.TipoEquipamento,
                        HardwareDeSala = item.Equipamento.IdHardwareDeSala.HasValue ?
                                        (uint)item.Equipamento.IdHardwareDeSala : 0,
                        IdModeloEquipamento = item.Equipamento.IdModeloEquipamento
                    };
                })
                .ToList();
        }


        /// <summary>
        /// Insere um novo equipamento.
        /// </summary>
        /// <param name="entity">Modelo do equipamento a ser inserido.</param>
        /// <returns>True se a inserção for bem-sucedida, caso contrário, false.</returns>
        public bool Insert(EquipamentoViewModel entity)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);
                IMonitoramentoService monitoramentoService = new MonitoramentoService(_context);

                var equip = SetEntity(entity.EquipamentoModel);

                _context.Add(equip);
                int inserted = _context.SaveChanges();
                _context.Entry(equip).Reload();
                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (inserted == 1)
                {
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdModeloEquipamento = (uint)equip.Id, IdOperacao = c.IdOperacao }));
                    codigoInfravermelhoService.AddAll(codigosEntity);

                    monitoramentoService.Insert(new MonitoramentoModel { Estado = false, EquipamentoId = equip.Id }); ;
                }
                return Convert.ToBoolean(inserted);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        /// <summary>
        /// Atualiza os dados de um equipamento existente.
        /// </summary>
        /// <param name="entity">Modelo do equipamento com os novos dados.</param>
        /// <returns>True se a atualização for bem-sucedida, caso contrário, false.</returns>
        public bool Update(EquipamentoViewModel entity)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);

                var equip = SetEntity(entity.EquipamentoModel);
                _context.Equipamentos.Update(equip);
                int updated = _context.SaveChanges();
                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (updated == 1)
                {
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdModeloEquipamento = (uint)equip.Id, IdOperacao = c.IdOperacao }));
                    codigoInfravermelhoService.UpdateAll(codigosEntity);
                }
                return Convert.ToBoolean(updated);


            }
            catch (Exception e)
            {

                throw e;
            }

        }

        /// <summary>
        /// Mapeia os dados de um modelo para uma entidade de equipamento.
        /// </summary>
        /// <param name="model">Modelo com os dados do equipamento.</param>
        /// <returns>Entidade preenchida com os dados do modelo.</returns>
        private static Equipamento SetEntity(EquipamentoModel model)
        {
            Equipamento entity = new Equipamento
            {
                Id = model.Id,
                Descricao = model.Descricao,
                IdModeloEquipamento = model.IdModeloEquipamento,            
                TipoEquipamento = model.TipoEquipamento,
                IdSala = model.Sala,
                IdHardwareDeSala = model.HardwareDeSala,
            };
            return entity;
        }

        /// <summary>
        /// Remove um equipamento pelo ID.
        /// </summary>
        /// <param name="id">ID do equipamento a ser removido.</param>
        /// <returns>True se a remoção for bem-sucedida, caso contrário, false.</returns>
        public bool Remove(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var equipamento = _context.Equipamentos.FirstOrDefault(e => e.Id == id);

                    if (equipamento == null)
                    {
                        throw new ServiceException("Equipamento não encontrado.");
                    }

                    var monitoramentos = _context.Monitoramentos.Where(m => m.IdEquipamento == id).ToList();

                    if (monitoramentos.Any())
                    {
                        _context.Monitoramentos.RemoveRange(monitoramentos);
                        _context.SaveChanges();
                    }

                    var codigosInfravermelho = _context.Codigoinfravermelhos.Where(ci => ci.IdModeloEquipamento == id).ToList();

                    if (codigosInfravermelho.Any())
                    {
                        _context.Codigoinfravermelhos.RemoveRange(codigosInfravermelho);
                        _context.SaveChanges();
                    }

                    _context.Equipamentos.Remove(equipamento);
                    var save = _context.SaveChanges() > 0;

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
    }
}
