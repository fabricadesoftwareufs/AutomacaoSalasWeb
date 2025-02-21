using Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Persistence;
using Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ConexaoInternetService : IConexaoInternetService
    {

        private readonly SalasDBContext _context;

        public ConexaoInternetService(SalasDBContext context)
        {
            _context = context;
        }

        public List<ConexaointernetModel> GetAll()
            => _context.Conexaointernets.AsNoTracking()
                                         .Select(c => new ConexaointernetModel { Id = c.Id, IdBloco = c.IdBloco, Nome = c.Nome, Senha = c.Senha })
                                         .ToList();

        public ConexaointernetModel GetById(uint id)
        {
            try
            {
                var conexao = (from c in _context.Conexaointernets
                               join b in _context.Blocos on c.IdBloco equals b.Id
                               where c.Id == id
                               select new ConexaointernetModel
                               {
                                   Id = c.Id,
                                   Nome = c.Nome,
                                   Senha = c.Senha,
                                   IdBloco = c.IdBloco,
                                   NomeBloco = b.Titulo
                               }).AsNoTracking().FirstOrDefault();

                if (conexao == null)
                {
                    throw new ConexaoInternetException($"Conexão de internet com ID {id} não encontrada.");
                }

                return conexao;
            }
            catch (Exception ex)
            {
                throw new ConexaoInternetException($"Erro ao buscar conexão de internet com ID {id}", ex);
            }
        }

        public List<ConexaointernetModel> GetByIdBloco(uint idBloco)
            => _context.Conexaointernets.AsNoTracking()
                                        .Where(c => c.IdBloco == idBloco)
                                        .Select(c => new ConexaointernetModel { Id = c.Id, IdBloco = c.IdBloco, Nome = c.Nome, Senha = c.Senha })
                                        .ToList();

        public List<ConexaointernetModel> GetByName(string name)
            => _context.Conexaointernets.AsNoTracking()
                                        .Where(c => c.Nome.ToUpper().Contains(name.ToUpper()))
                                        .OrderBy(c => c.Nome)
                                        .Select(c => new ConexaointernetModel { Id = c.Id, IdBloco = c.IdBloco, Nome = c.Nome, Senha = c.Senha })
                                        .ToList();

        public bool Remove(uint id)
        {
            try
            {
                var conexao = _context.Conexaointernets.Find(id);
                if (conexao == null)
                {
                    throw new ConexaoInternetException("Conexão de internet não encontrada.");
                }

                _context.Conexaointernets.Remove(conexao);
                return _context.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new ConexaoInternetException("Erro ao remover a conexão de internet.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ConexaoInternetException("Erro inesperado ao remover a conexão de internet.", ex);
            }
        }

        public bool Insert(ConexaointernetModel conexao)
        {
            try
            {
                _context.Conexaointernets.Add(new Conexaointernet
                {
                    Nome = conexao.Nome,
                    Senha = conexao.Senha,
                    IdBloco = conexao.IdBloco
                });
                return _context.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new ConexaoInternetException("Erro ao inserir a conexão de internet.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ConexaoInternetException("Erro inesperado ao inserir a conexão de internet.", ex);
            }
        }

        public bool Update(ConexaointernetModel conexao)
        {
            try
            {
                var conexaoUpdate = _context.Conexaointernets.Find(conexao.Id);
                if (conexaoUpdate == null)
                {
                    throw new ConexaoInternetException("Conexão de internet não encontrada.");
                }

                conexaoUpdate.Nome = conexao.Nome;
                conexaoUpdate.Senha = conexao.Senha;
                conexaoUpdate.IdBloco = conexao.IdBloco;
                return _context.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new ConexaoInternetException("Erro ao atualizar a conexão de internet.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ConexaoInternetException("Erro inesperado ao atualizar a conexão de internet.", ex);
            }
 
        }
    }
}