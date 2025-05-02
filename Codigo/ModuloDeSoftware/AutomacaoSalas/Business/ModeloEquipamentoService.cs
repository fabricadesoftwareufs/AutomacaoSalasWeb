using Microsoft.EntityFrameworkCore;
using Model;
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
    internal class ModeloEquipamentoService : IModeloEquipamentoService
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

            if (modelo == null || !modelo.Any())
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
        public bool Insert(ModeloEquipamentoModel modelo)
        {
            try
            {
                var entity = SetEntity(modelo);
                _context.Modeloequipamentos.Add(entity);
                _context.SaveChanges();
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
            try
            {
                var modelo = _context.Modeloequipamentos.FirstOrDefault(m => m.Id == id);
                if (modelo == null)
                {
                    throw new ModeloEquipamentoException($"Modelo de Equipamento com ID {id} não encontrado.");
                }
                _context.Modeloequipamentos.Remove(modelo);
                return _context.SaveChanges() == 1;
            }
            catch (DbUpdateException ex)
            {
                throw new ModeloEquipamentoException("Erro ao remover o modelo de equipamento.", ex);
            }
            catch (Exception ex)
            {
                throw new ModeloEquipamentoException("Erro inesperado ao remover o modelo de equipamento.", ex);
            }
        }

        /// <summary>
        /// Atualiza as informações de um modelo de equipamento existente no sistema.
        /// </summary>
        /// <param name="modelo">Modelo de equipamento com as informações atualizadas.</param>
        /// <returns>True se a atualização for bem-sucedida; caso contrário, False.</returns>
        /// <exception cref="ModeloEquipamentoException">Lançada quando o modelo não é encontrado ou ocorre um erro durante a atualização.</exception>
        public bool Update(ModeloEquipamentoModel modelo)
        {
            try
            {
                var modeloExistente = _context.Modeloequipamentos.Find(modelo.Id);
                if (modeloExistente == null)
                {
                    throw new ModeloEquipamentoException($"Modelo de Equipamento com ID {modelo.Id} não encontrado.");
                }
                modeloExistente.Nome = modelo.Nome;
                modeloExistente.IdMarcaEquipamento = modelo.MarcaEquipamentoID;
                return _context.SaveChanges() == 1;
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