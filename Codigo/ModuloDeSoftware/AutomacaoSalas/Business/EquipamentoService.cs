using Model;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly SalasDBContext _context;

        public EquipamentoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna um equipamento pelo seu ID.
        /// </summary>
        /// <param name="idEquipamento">ID do equipamento.</param>
        /// <returns>Um modelo de equipamento.</returns>
        public EquipamentoModel GetByIdEquipamento(int idEquipamento)
            => _context.Equipamentos
                       .Where(eq => eq.Id == idEquipamento)
                       .Select(eq => new EquipamentoModel
                       {
                           Id = eq.Id,
                           Modelo = eq.Modelo,
                           Marca = eq.Marca,
                           Descricao = eq.Descricao,
                           Sala = eq.Sala,
                           TipoEquipamento = eq.TipoEquipamento,
                           HardwareDeSala = (uint)eq.HardwareDeSala
                       }).FirstOrDefault();

        /// <summary>
        /// Retorna um equipamento com base no ID da sala e no tipo de equipamento.
        /// </summary>
        /// <param name="idSala">ID da sala.</param>
        /// <param name="tipo">Tipo de equipamento.</param>
        /// <returns>Um modelo de equipamento.</returns>
        public EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo)
            => _context.Equipamentos
                       .Where(eq => eq.Sala == idSala && eq.TipoEquipamento.ToUpper().Equals(tipo.ToUpper()))
                       .Select(eq => new EquipamentoModel
                       {
                           Id = eq.Id,
                           Modelo = eq.Modelo,
                           Marca = eq.Marca,
                           Descricao = eq.Descricao,
                           Sala = eq.Sala,
                           TipoEquipamento = eq.TipoEquipamento,
                           HardwareDeSala = (uint)eq.HardwareDeSala
                       }).FirstOrDefault();

        /// <summary>
        /// Retorna todos os equipamentos de uma sala específica.
        /// </summary>
        /// <param name="idSala">ID da sala.</param>
        /// <returns>Lista de modelos de equipamentos.</returns>
        public List<EquipamentoModel> GetByIdSala(int idSala)
            => _context.Equipamentos
                       .Where(eq => eq.Sala == idSala)
                       .Select(eq => new EquipamentoModel
                       {
                           Id = eq.Id,
                           Modelo = eq.Modelo,
                           Marca = eq.Marca,
                           Descricao = eq.Descricao,
                           Sala = eq.Sala,
                           TipoEquipamento = eq.TipoEquipamento,
                           HardwareDeSala = (uint)eq.HardwareDeSala
                       }).ToList();

        /// <summary>
        /// Retorna todos os equipamentos cadastrados.
        /// </summary>
        /// <returns>Lista de modelos de equipamentos.</returns>
        public List<EquipamentoModel> GetAll()
            => _context.Equipamentos.Select(e => new EquipamentoModel
            {
                Id = e.Id,
                Modelo = e.Modelo,
                Descricao = e.Descricao,
                TipoEquipamento = e.TipoEquipamento,
                Marca = e.Marca,
                Sala = e.Sala,
                HardwareDeSala = e.HardwareDeSala != null ? (uint)e.HardwareDeSala : 0
            }).ToList();

        /// <summary>
        /// Insere um novo equipamento no banco de dados.
        /// </summary>
        /// <param name="entity">Dados do equipamento a ser inserido.</param>
        /// <returns>Retorna true se a inserção for bem-sucedida, caso contrário, false.</returns>
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
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdEquipamento = equip.Id, IdOperacao = c.IdOperacao }));
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
        /// Atualiza um equipamento existente.
        /// </summary>
        /// <param name="entity">Dados do equipamento a ser atualizado.</param>
        /// <returns>Retorna true se a atualização for bem-sucedida, caso contrário, false.</returns>
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
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdEquipamento = equip.Id, IdOperacao = c.IdOperacao }));
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
        /// Converte um modelo de equipamento em uma entidade de equipamento.
        /// </summary>
        /// <param name="model">Modelo do equipamento.</param>
        /// <returns>Entidade do equipamento.</returns>
        private static Equipamento SetEntity(EquipamentoModel model)
        {
            Equipamento entity = new Equipamento
            {
                Id = model.Id,
                Descricao = model.Descricao,
                Marca = model.Marca,
                Modelo = model.Modelo,
                TipoEquipamento = model.TipoEquipamento,
                Sala = model.Sala,
                HardwareDeSala = model.HardwareDeSala,
            };
            return entity;
        }

        /// <summary>
        /// Remove um equipamento do banco de dados.
        /// </summary>
        /// <param name="id">ID do equipamento a ser removido.</param>
        /// <returns>Retorna true se a remoção for bem-sucedida, caso contrário, false.</returns>
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

                    var monitoramentos = _context.Monitoramentos.Where(m => m.Equipamento == id).ToList();

                    if (monitoramentos.Any())
                    {
                        _context.Monitoramentos.RemoveRange(monitoramentos);
                        _context.SaveChanges();
                    }

                    var codigosInfravermelho = _context.Codigoinfravermelhos.Where(ci => ci.Equipamento == id).ToList();

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
