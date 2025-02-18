using Microsoft.VisualBasic.FileIO;
using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Service
{
    public class ConexaoInternetSalaService : IConexaoInternetSalaService
    {
        private readonly SalasDBContext _context;

        public ConexaoInternetSalaService(SalasDBContext context)
        {
            _context = context;
        }

        public List<ConexaoInternetSalaModel> GetAll() => _context.Conexaointernetsalas.Select(cis => new ConexaoInternetSalaModel { ConexaoInternetId = cis.IdConexaoInternet, SalaId = cis.IdSala, Prioridade = cis.Prioridade }).ToList();


        public ConexaoInternetSalaModel GetById(uint idConexaoInternet, uint idSala) => _context.Conexaointernetsalas.Where(cis => cis.IdConexaoInternet == idConexaoInternet && cis.IdSala == idSala).Select(cis => new ConexaoInternetSalaModel { ConexaoInternetId = cis.IdConexaoInternet, SalaId = cis.IdSala, Prioridade = cis.Prioridade }).FirstOrDefault();


        public List<ConexaoInternetSalaModel> GetByIdConexaoInternet(uint id) => _context.Conexaointernetsalas.Where(cis => cis.IdConexaoInternet == id).Select(cis => new ConexaoInternetSalaModel { ConexaoInternetId = cis.IdConexaoInternet, SalaId = cis.IdSala, Prioridade = cis.Prioridade }).ToList();


        public List<ConexaoInternetSalaModel> GetByIdSala(uint id) => _context.Conexaointernetsalas.Where(cis => cis.IdSala == id).Select(cis => new ConexaoInternetSalaModel { SalaId = cis.IdSala, ConexaoInternetId = cis.IdConexaoInternet, Prioridade = cis.Prioridade }).ToList();

        public List<ConexaoInternetSalaModel> GetBySalaOrdenadoPorPrioridade(uint idSala) => _context.Conexaointernetsalas.Where(cis => cis.IdSala == idSala).OrderBy(cis => cis.Prioridade).Select(cis => new ConexaoInternetSalaModel { ConexaoInternetId = cis.IdConexaoInternet, SalaId = cis.IdSala, Prioridade = cis.Prioridade}).ToList();


        private static Conexaointernetsala SetEntity(ConexaoInternetSalaModel model) => new Conexaointernetsala()
        {
            IdConexaoInternet = model.ConexaoInternetId,
            IdSala = model.SalaId,
            Prioridade = model.Prioridade,
        };

        public bool Insert(ConexaoInternetSalaModel entity)
        {
            var conexaoSala = SetEntity(entity);
            _context.Add(conexaoSala);
            var ok = _context.SaveChanges();
            return ok == 1 ? true : false;
        }

        public bool Remove(uint idConexaoInternet, uint idSala)
        {
            var x = _context.Conexaointernetsalas.Where(cis => cis.IdConexaoInternet == idConexaoInternet && cis.IdSala == idSala).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1;
            }

            return false;
        }
        public bool Update(ConexaoInternetSalaModel entity)
        {
            var x = _context.Conexaointernetsalas.Where(cis => cis.IdConexaoInternet == entity.ConexaoInternetId && cis.IdSala == entity.SalaId).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity));
                return _context.SaveChanges() == 1;
            }

            return false;
        }

        public bool RemoveByConexaoInternet(uint idConexaoInternet)
        {
            var x = _context.Conexaointernetsalas.Where(cis => cis.IdConexaoInternet == idConexaoInternet);
            if (x != null)
            {
                _context.RemoveRange(x);
                return _context.SaveChanges() > 0;
            }

            return false;

        }

        public bool MoverPrioridade(uint idSala, uint idConexaoInternet, int novaPosicao)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Pega lista atual ordenada
                    var conexoes = _context.Conexaointernetsalas
                        .Where(cis => cis.IdSala == idSala)
                        .OrderBy(cis => cis.Prioridade)
                        .ToList();

                    if (!conexoes.Any())
                        throw new ServiceException("Nenhuma conexão encontrada para esta sala.");

                    // Encontra posição atual
                    var itemAtual = conexoes.FirstOrDefault(x => x.IdConexaoInternet == idConexaoInternet);
                    if (itemAtual == null)
                        throw new ServiceException("Conexão específica não encontrada.");

                    int antigaPosicao = itemAtual.Prioridade;

                    // Ajusta as prioridades
                    foreach (var conexao in conexoes)
                    {
                        if (conexao.IdConexaoInternet == idConexaoInternet)
                        {
                            conexao.Prioridade = novaPosicao;
                        }
                        else if (novaPosicao <= antigaPosicao &&
                                conexao.Prioridade >= novaPosicao &&
                                conexao.Prioridade < antigaPosicao)
                        {
                            conexao.Prioridade++;
                        }
                        else if (novaPosicao >= antigaPosicao &&
                                conexao.Prioridade <= novaPosicao &&
                                conexao.Prioridade > antigaPosicao)
                        {
                            conexao.Prioridade--;
                        }
                    }

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

        public bool TrocarPrioridade(uint idSala, uint idConexaoInternet1, uint idConexaoInternet2)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var conexao1 = _context.Conexaointernetsalas
                        .FirstOrDefault(c => c.IdSala == idSala && c.IdConexaoInternet == idConexaoInternet1);

                    var conexao2 = _context.Conexaointernetsalas
                        .FirstOrDefault(c => c.IdSala == idSala && c.IdConexaoInternet == idConexaoInternet2);

                    if (conexao1 == null || conexao2 == null)
                        throw new ServiceException("Uma ou mais conexões não encontradas.");

                    // Troca as prioridades
                    var tempPrioridade = conexao1.Prioridade;
                    conexao1.Prioridade = conexao2.Prioridade;
                    conexao2.Prioridade = tempPrioridade;

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
