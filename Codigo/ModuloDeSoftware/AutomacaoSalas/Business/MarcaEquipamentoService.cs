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
    /// Serviço responsável por gerenciar operações relacionadas às marcas de equipamentos
    /// </summary>
    public class MarcaEquipamentoService : IMarcaEquipamentoService
    {
        private readonly SalasDBContext _context;
        private readonly ModeloEquipamentoService _modeloService;
        /// <summary>
        /// Construtor do serviço de marcas de equipamentos
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        public MarcaEquipamentoService(SalasDBContext context)
        {
            _context = context;
            _modeloService = new ModeloEquipamentoService(context);
        }

        /// <summary>
        /// Retorna todas as marcas de equipamentos cadastradas
        /// </summary>
        public List<MarcaEquipamentoModel> GetAll() =>
            _context.Marcaequipamentos.Select(m => new MarcaEquipamentoModel { Id = m.Id, Nome = m.Nome }).ToList();

        /// <summary>
        /// Busca marcas de equipamentos por nome (busca parcial)
        /// </summary>
        /// <param name="name">Nome ou parte do nome a ser pesquisado</param>
        public List<MarcaEquipamentoModel> GetByName(string name) =>
            _context.Marcaequipamentos.Where(m => m.Nome.Contains(name))
                                      .Select(m => new MarcaEquipamentoModel { Id = m.Id, Nome = m.Nome })
                                      .ToList();

        /// <summary>
        /// Obtém uma marca de equipamento pelo seu ID
        /// </summary>
        /// <param name="id">ID da marca a ser buscada</param>
        /// <exception cref="MarcaEquipamentoException">Lançada quando a marca não é encontrada</exception>
        public MarcaEquipamentoModel GetById(uint id)
        {
            var marca = _context.Marcaequipamentos.Where(m => m.Id == id)
                                                 .Select(m => new MarcaEquipamentoModel { Id = m.Id, Nome = m.Nome })
                                                 .FirstOrDefault();
            if (marca == null)
            {
                throw new MarcaEquipamentoException($"Marca de Equipamento com ID {id} não encontrada.");
            }
            return marca;
        }

        /// <summary>
        /// Converte um modelo para a entidade correspondente
        /// </summary>
        /// <param name="model">Modelo a ser convertido</param>
        private static Marcaequipamento SetEntity(MarcaEquipamentoModel model) => new Marcaequipamento()
        {
            Id = model.Id,
            Nome = model.Nome
        };

        /// <summary>
        /// Insere uma nova marca de equipamento
        /// </summary>
        /// <param name="marca">Modelo da marca a ser inserida</param>
        /// <returns>True se a inserção foi bem-sucedida, false caso contrário</returns>
        /// <exception cref="MarcaEquipamentoException">Lançada quando ocorre um erro na inserção</exception>
        public bool Insert(MarcaEquipamentoModel marca)
        {
            try
            {
                var marcaEquipamento = SetEntity(marca);
                _context.Add(marcaEquipamento);
                var ok = _context.SaveChanges();
                return ok == 1;
            }
            catch (DbUpdateException dbEx)
            {
                throw new MarcaEquipamentoException("Erro ao inserir a marca de equipamento.", dbEx);
            }
            catch (Exception ex)
            {
                throw new MarcaEquipamentoException("Erro inesperado ao inserir a marca de equipamento.", ex);
            }
        }

        /// <summary>
        /// Remove uma marca de equipamento pelo ID
        /// </summary>
        /// <param name="id">ID da marca a ser removida</param>
        /// <returns>True se a remoção foi bem-sucedida, false caso contrário</returns>
        /// <exception cref="MarcaEquipamentoException">Lançada quando ocorre um erro na remoção ou a marca não é encontrada</exception>
        public bool Remove(uint id)
        {
            try
            {
                if (_modeloService.GetByMarca(id).Count == 0)
                {
                    var marcaEquipamento = _context.Marcaequipamentos.FirstOrDefault(m => m.Id == id);
                    if (marcaEquipamento == null)
                    {
                        throw new MarcaEquipamentoException($"Marca de Equipamento com ID {id} não encontrada.");
                    }
                    _context.Remove(marcaEquipamento);
                    return _context.SaveChanges() == 1;
                }
                else
                {
                    throw new MarcaEquipamentoException("Esta marca não pode ser removida, pois possui modelos de equipamentos associados a ela.");
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new MarcaEquipamentoException("Erro ao remover a marca de equipamento.", dbEx);
            }
            catch (MarcaEquipamentoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MarcaEquipamentoException("Erro inesperado ao remover a marca de equipamento.", ex);
            }
        }

        /// <summary>
        /// Atualiza uma marca de equipamento existente
        /// </summary>
        /// <param name="marca">Modelo com os dados atualizados da marca</param>
        /// <returns>True se a atualização foi bem-sucedida, false caso contrário</returns>
        /// <exception cref="MarcaEquipamentoException">Lançada quando ocorre um erro na atualização ou a marca não é encontrada</exception>
        public bool Update(MarcaEquipamentoModel marca)
        {
            try
            {
                var marcaUpdate = _context.Marcaequipamentos.Find(marca.Id);
                if (marcaUpdate == null)
                {
                    throw new MarcaEquipamentoException("Marca de Equipamento não encontrada.");
                }
                marcaUpdate.Nome = marca.Nome;
                return _context.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new MarcaEquipamentoException("Erro ao atualizar a marca de equipamento.", dbEx);
            }
            catch (Exception ex)
            {
                throw new MarcaEquipamentoException("Erro inesperado ao atualizar a marca de equipamento.", ex);
            }
        }
    }
}