using Microsoft.EntityFrameworkCore;
using Model;
using Model.ViewModel;
using Persistence;
using Service.Exceptions;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Serviço responsável pela gestão de modelos de equipamentos no sistema.
    /// Implementa a interface IModeloEquipamentoService e fornece funcionalidades
    /// para realizar operações CRUD relacionadas aos modelos de equipamentos.
    /// </summary>
    public class ModeloEquipamentoService : IModeloEquipamentoService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Inicializa uma nova instância da classe ModeloEquipamentoService.
        /// </summary>
        /// <param name="context">Contexto do banco de dados para acesso aos dados.</param>
        public ModeloEquipamentoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recupera todos os modelos de equipamentos cadastrados no sistema.
        /// </summary>
        /// <returns>Lista contendo todos os modelos de equipamentos.</returns>
        public List<ModeloEquipamentoModel> GetAll() => _context.Modeloequipamentos.Select(m => new ModeloEquipamentoModel { Id = m.Id, Nome = m.Nome, MarcaEquipamentoID = m.IdMarcaEquipamento }).ToList();

        /// <summary>
        /// Recupera um modelo de equipamento específico pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador único do modelo de equipamento.</param>
        /// <returns>Modelo de equipamento correspondente ao ID fornecido.</returns>
        /// <exception cref="ModeloEquipamentoException">Lançada quando o modelo não é encontrado.</exception>
        public ModeloEquipamentoModel GetById(uint id)
        {
            var modelo = _context.Modeloequipamentos
                .Where(m => m.Id == id)
                .Select(m => new ModeloEquipamentoModel { Id = m.Id, Nome = m.Nome, MarcaEquipamentoID = m.IdMarcaEquipamento })
                .FirstOrDefault();

            if (modelo == null)
            {
                throw new ModeloEquipamentoException($"Modelo de Equipamento com ID {id} não encontrado.");
            }

            return modelo;
        }

        /// <summary>
        /// Recupera todos os modelos de equipamentos associados a uma marca específica.
        /// </summary>
        /// <param name="idMarca">Identificador único da marca de equipamento.</param>
        /// <returns>Lista de modelos de equipamentos da marca especificada.</returns>
        /// <exception cref="ModeloEquipamentoException">Lançada quando nenhum modelo é encontrado para a marca.</exception>
        public List<ModeloEquipamentoModel> GetByMarca(uint idMarca)
        {
            var modelo = _context.Modeloequipamentos
                .Where(m => m.IdMarcaEquipamento == idMarca)
                .Select(m => new ModeloEquipamentoModel { Id = m.Id, Nome = m.Nome, MarcaEquipamentoID = m.IdMarcaEquipamento })
                .ToList();

            if (!modelo.Any())
            {
                throw new ModeloEquipamentoException($"Nenhum modelo de equipamento encontrado para a marca com ID {idMarca}.");
            }

            return modelo;
        }

        /// <summary>
        /// Recupera todos os modelos de equipamentos que contenham o nome especificado.
        /// </summary>
        /// <param name="name">Nome ou parte do nome a ser pesquisado.</param>
        /// <returns>Lista de modelos de equipamentos que correspondem ao critério de pesquisa.</returns>
        /// <exception cref="ModeloEquipamentoException">Lançada quando nenhum modelo é encontrado com o nome especificado.</exception>
        public List<ModeloEquipamentoModel> GetByName(string name)
        {
            var modelo = _context.Modeloequipamentos
                .Where(m => m.Nome.Contains(name))
                .Select(m => new ModeloEquipamentoModel { Id = m.Id, Nome = m.Nome, MarcaEquipamentoID = m.IdMarcaEquipamento })
                .ToList();

            if (modelo == null || !modelo.Any())
            {
                throw new ModeloEquipamentoException($"Nenhum modelo de equipamento encontrado com o nome '{name}'.");
            }

            return modelo;
        }

        /// <summary>
        /// Converte um modelo de domínio para uma entidade de banco de dados.
        /// </summary>
        /// <param name="model">Modelo de domínio a ser convertido.</param>
        /// <returns>Entidade de banco de dados correspondente ao modelo.</returns>
        private static Modeloequipamento SetEntity(ModeloEquipamentoModel model) => new Modeloequipamento()
        {
            Id = model.Id,
            Nome = model.Nome,
            IdMarcaEquipamento = model.MarcaEquipamentoID
        };

        /// <summary>
        /// Insere um novo modelo de equipamento no sistema.
        /// </summary>
        /// <param name="modelo">Modelo de equipamento a ser inserido.</param>
        /// <returns>True se a inserção for bem-sucedida; caso contrário, False.</returns>
        /// <exception cref="ModeloEquipamentoException">Lançada quando ocorre um erro durante a inserção.</exception>
        // Trecho problemático em ModeloEquipamentoService.cs
        public bool Insert(ModeloEquipamentoViewModel modelo)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);

                var modeloEquipamento = SetEntity(modelo.ModeloEquipamento);

                _context.Modeloequipamentos.Add(modeloEquipamento);
                int inserted = _context.SaveChanges();
                _context.Entry(modeloEquipamento).Reload(); // Recarregar para obter o ID gerado

                if (inserted > 0 && modelo.Codigos != null && modelo.Codigos.Any())
                {
                    var codigosEntity = new List<CodigoInfravermelhoModel>();

                    foreach (var codigo in modelo.Codigos)
                    {
                        codigosEntity.Add(new CodigoInfravermelhoModel
                        {
                            Codigo = codigo.Codigo,
                            IdModeloEquipamento = modeloEquipamento.Id, // Usado o ID gerado automaticamente
                            IdOperacao = codigo.IdOperacao
                        });
                    }

                    bool codigosSalvos = codigoInfravermelhoService.AddAll(codigosEntity);
                    if (!codigosSalvos)
                        throw new ModeloEquipamentoException("Falha ao salvar os códigos de operação.");
                }

                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new ModeloEquipamentoException("Erro ao inserir o modelo de equipamento.", ex);
            }
            catch (Exception ex)
            {
                throw new ModeloEquipamentoException("Erro inesperado ao inserir o modelo de equipamento.", ex);
            }
        }

        /// <summary>
        /// Remove um modelo de equipamento do sistema com base no ID fornecido.
        /// </summary>
        /// <param name="id">Identificador único do modelo de equipamento a ser removido.</param>
        /// <returns>True se a remoção for bem-sucedida; caso contrário, False.</returns>
        /// <exception cref="ModeloEquipamentoException">Lançada quando o modelo não é encontrado ou ocorre um erro durante a remoção.</exception>
        public bool Remove(uint id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var modeloEquipamento = _context.Modeloequipamentos.FirstOrDefault(m => m.Id == id);
                    var marcaModelo = _context.Marcaequipamentos.FirstOrDefault(m => m.Id == modeloEquipamento.IdMarcaEquipamento);
                    if (modeloEquipamento == null)
                    {
                        throw new ModeloEquipamentoException($"Modelo de Equipamento com ID {id} não encontrado.");
                    }

                    // Verificar se existem equipamentos associados a este modelo
                    bool possuiEquipamentosAssociados = _context.Equipamentos.Any(e => e.IdModeloEquipamento == id);

                    if (possuiEquipamentosAssociados)
                    {
                        throw new ModeloEquipamentoException($"Não é possível excluir o Modelo de equipamento {modeloEquipamento.Nome} da Marca {marcaModelo.Nome} pois ainda existem equipamentos associados a ele.");
                    }

                    var codigoInfravermelho = _context.Codigoinfravermelhos.Where(m => m.IdModeloEquipamento == id).ToList();

                    if (codigoInfravermelho.Any())
                    {
                        _context.Codigoinfravermelhos.RemoveRange(codigoInfravermelho);
                        _context.SaveChanges();
                    }

                    _context.Modeloequipamentos.Remove(modeloEquipamento);
                    var save = _context.SaveChanges() > 0;

                    transaction.Commit();
                    return save;
                }
                catch (DbUpdateException ex)
                {
                    throw new DbUpdateException("Erro ao remover o modelo de equipamento.", ex);
                }
            }
        }


        /// <summary>
        /// Atualiza as informações de um modelo de equipamento existente no sistema.
        /// </summary>
        /// <param name="modelo">Modelo de equipamento com as informações atualizadas.</param>
        /// <returns>True se a atualização for bem-sucedida; caso contrário, False.</returns>
        /// <exception cref="ModeloEquipamentoException">Lançada quando o modelo não é encontrado ou ocorre um erro durante a atualização.</exception>
        public bool Update(ModeloEquipamentoViewModel modelo)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);

                var modeloEquipamento = SetEntity(modelo.ModeloEquipamento);
                _context.Modeloequipamentos.Update(modeloEquipamento);
                int updated = _context.SaveChanges();
                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (updated == 1)
                {
                    modelo.Codigos.ForEach(m => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = m.Codigo, IdModeloEquipamento = (uint)modeloEquipamento.Id, IdOperacao = m.IdOperacao }));
                    codigoInfravermelhoService.UpdateAll(codigosEntity);
                }
                return Convert.ToBoolean(updated);
            }
            catch (DbUpdateException ex)
            {
                throw new ModeloEquipamentoException("Erro ao atualizar o modelo de equipamento.", ex);
            }
            catch (Exception ex)
            {
                throw new ModeloEquipamentoException("Erro inesperado ao atualizar o modelo de equipamento.", ex);
            }
        }
    }
}