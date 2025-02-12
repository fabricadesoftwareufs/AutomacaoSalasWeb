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


        public ConexaoInternetSalaModel GetById(uint idConexaoInternet, uint idSala) => _context.Conexaointernetsalas.Where(cis => cis.IdConexaoInternet == idConexaoInternet && cis.IdSala == idSala).Select(cis => new ConexaoInternetSalaModel{ ConexaoInternetId = cis.IdConexaoInternet, SalaId = cis.IdSala}).FirstOrDefault();
               

        public List<ConexaoInternetSalaModel> GetByIdConexaoInternet(uint id) => _context.Conexaointernetsalas.Where(cis => cis.IdConexaoInternet == id).Select(cis => new ConexaoInternetSalaModel { ConexaoInternetId = cis.IdConexaoInternet}).ToList();
        

        public List<ConexaoInternetSalaModel> GetByIdSala(uint id) => _context.Conexaointernetsalas.Where(cis => cis.IdSala == id).Select(cis => new ConexaoInternetSalaModel {SalaId = cis.IdSala }).ToList();

        public List<ConexaoInternetSalaModel> GetBySalaOrdenadoPorPrioridade(uint idSala)
        {
            try
            {
                using (var connection = _databaseConnection.Connection())
                {
                    var sql = @"SELECT ConexaoInternetId, SalaId, Prioridade 
                       FROM ConexaoInternetSala 
                       WHERE SalaId = @idSala 
                       ORDER BY Prioridade";

                    return connection.Query<ConexaoInternetSalaModel>(sql, new { idSala }).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar conexões ordenadas por prioridade");
                return new List<ConexaoInternetSalaModel>();
            }
        }

        public bool Insert(ConexaoInternetSalaModel entity)
        {
            throw new NotImplementedException();
        }

        public bool MoverPrioridade(uint idSala, uint idConexaoInternet, uint novaPosicao)
        {
            try
            {
                using (var connection = _databaseConnection.Connection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        // Pega lista atual ordenada
                        var lista = GetBySalaOrdenadoPorPrioridade(idSala);
                        if (!lista.Any()) return false;

                        // Encontra posição atual
                        var itemAtual = lista.FirstOrDefault(x => x.ConexaoInternetId == idConexaoInternet);
                        if (itemAtual == null) return false;

                        // Ajusta todas as prioridades
                        var sql = @"UPDATE ConexaoInternetSala 
                           SET Prioridade = CASE
                               WHEN ConexaoInternetId = @idConexaoInternet THEN @novaPosicao
                               WHEN Prioridade >= @novaPosicao AND Prioridade < @antigaPosicao THEN Prioridade + 1
                               WHEN Prioridade <= @novaPosicao AND Prioridade > @antigaPosicao THEN Prioridade - 1
                               ELSE Prioridade
                           END
                           WHERE SalaId = @idSala";

                        connection.Execute(sql, new
                        {
                            idSala,
                            idConexaoInternet,
                            novaPosicao,
                            antigaPosicao = itemAtual.Prioridade
                        }, transaction);

                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao mover prioridade");
                return false;
            }
        }

        public bool Remove(uint idConexaoInternet, uint idSala)
        {
            throw new NotImplementedException();
        }

        public bool RemoveByConexaoInternet(uint idConexaoInternet)
        {
            throw new NotImplementedException();
        }

        public bool TrocarPrioridade(uint idSala, uint idConexaoInternet1, uint idConexaoInternet2)
        {
            try
            {
                using (var connection = _databaseConnection.Connection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        // Pega as duas conexões
                        var sql = @"SELECT ConexaoInternetId, SalaId, Prioridade 
                           FROM ConexaoInternetSala 
                           WHERE SalaId = @idSala 
                           AND ConexaoInternetId IN (@id1, @id2)";

                        var items = connection.Query<ConexaoInternetSalaModel>(sql,
                            new { idSala, id1 = idConexaoInternet1, id2 = idConexaoInternet2 })
                            .ToList();

                        if (items.Count != 2) return false;

                        // Troca as prioridades
                        var prioridade1 = items[0].Prioridade;
                        var prioridade2 = items[1].Prioridade;

                        sql = @"UPDATE ConexaoInternetSala 
                       SET Prioridade = CASE 
                           WHEN ConexaoInternetId = @id1 THEN @p2
                           WHEN ConexaoInternetId = @id2 THEN @p1
                       END
                       WHERE SalaId = @idSala 
                       AND ConexaoInternetId IN (@id1, @id2)";

                        connection.Execute(sql, new
                        {
                            idSala,
                            id1 = idConexaoInternet1,
                            id2 = idConexaoInternet2,
                            p1 = prioridade1,
                            p2 = prioridade2
                        }, transaction);

                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao trocar prioridades");
                return false;
            }
        }

        public bool Update(ConexaoInternetSalaModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
