using Model;
using MySql.Data.MySqlClient;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas a organizações no sistema.
    /// </summary>
    public class OrganizacaoService : IOrganizacaoService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Construtor do serviço de organização.
        /// </summary>
        /// <param name="context">Contexto do banco de dados para operações de persistência.</param>
        public OrganizacaoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recupera todas as organizações cadastradas no sistema.
        /// </summary>
        /// <returns>Lista de todas as organizações mapeadas para o modelo de organização.</returns>
        public List<OrganizacaoModel> GetAll() => _context.Organizacaos.Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).ToList();

        /// <summary>
        /// Recupera uma lista de organizações com base em uma lista de identificadores.
        /// </summary>
        /// <param name="ids">Lista de identificadores das organizações a serem recuperadas.</param>
        /// <returns>Lista de organizações correspondentes aos identificadores fornecidos.</returns>
        public List<OrganizacaoModel> GetInList(List<uint> ids) => _context.Organizacaos.Where(o => ids.Contains(o.Id)).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).ToList();

        /// <summary>
        /// Recupera uma organização pelo seu identificador único.
        /// </summary>
        /// <param name="id">Identificador da organização.</param>
        /// <returns>Modelo da organização encontrada ou null se não existir.</returns>
        public OrganizacaoModel GetById(uint id) => _context.Organizacaos.Where(o => o.Id == id).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).FirstOrDefault();

        /// <summary>
        /// Recupera uma organização pelo seu CNPJ.
        /// </summary>
        /// <param name="cnpj">CNPJ da organização.</param>
        /// <returns>Modelo da organização encontrada ou null se não existir.</returns>
        public OrganizacaoModel GetByCnpj(string cnpj) => _context.Organizacaos.Where(o => o.Cnpj.Equals(cnpj)).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).FirstOrDefault();

        /// <summary>
        /// Insere uma nova organização no sistema e cria automaticamente os tipos de hardware padrão.
        /// </summary>
        /// <param name="entity">Modelo da organização a ser inserida.</param>
        /// <returns>True se a inserção for bem-sucedida, false caso contrário.</returns>
        /// <exception cref="ServiceException">Lançada se já existir uma organização com o mesmo CNPJ.</exception>
        public bool Insert(OrganizacaoModel entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (GetByCnpj(entity.Cnpj) != null)
                        throw new ServiceException("Uma organização com este CNPJ já está cadastrada.");

                    // Insere a organização
                    var organizacaoEntity = SetEntity(entity, new Organizacao());
                    _context.Add(organizacaoEntity);

                    if (_context.SaveChanges() != 1)
                        throw new ServiceException("Erro ao inserir a organização.");

                    // Cria os tipos de hardware padrão para a nova organização
                    var tipoHardwareService = new TipoHardwareService(_context);

                    var tiposHardwarePadrao = new List<TipoHardwareModel>
            {
                new TipoHardwareModel
                {
                    Descricao = "CONTROLADOR DE SALA",
                    IdOrganizacao = organizacaoEntity.Id
                },
                new TipoHardwareModel
                {
                    Descricao = "MODULO DE SENSORIAMENTO",
                    IdOrganizacao = organizacaoEntity.Id
                },
                new TipoHardwareModel
                {
                    Descricao = "MODULO DE DISPOSITIVO",
                    IdOrganizacao = organizacaoEntity.Id
                }
            };

                    foreach (var tipoHardware in tiposHardwarePadrao)
                    {
                        if (!tipoHardwareService.Insert(tipoHardware))
                            throw new ServiceException($"Erro ao criar o tipo de hardware: {tipoHardware.Descricao}");
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Remove uma organização do sistema e todos os tipos de hardware associados.
        /// </summary>
        /// <param name="id">Identificador da organização a ser removida.</param>
        /// <returns>True se a remoção for bem-sucedida, false caso contrário.</returns>
        /// <exception cref="ServiceException">Lançada se existirem blocos ou usuários associados à organização.</exception>
        public bool Remove(uint id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var _blocoService = new BlocoService(_context);
                    var _usuarioOrganizacaoService = new UsuarioOrganizacaoService(_context);
                    var _tipoHardwareService = new TipoHardwareService(_context);

                    // Verifica se ainda existem blocos ou usuários associados à organização
                    var blocos = _blocoService.GetByIdOrganizacao(id);
                    var usuarios = _usuarioOrganizacaoService.GetByIdOrganizacao(id);

                    if (blocos.Count > 0 || usuarios.Count > 0)
                    {
                        throw new ServiceException("A organização não pode ser removida, pois ainda existem usuários ou blocos associados a ela.");
                    }

                    // Remove todos os tipos de hardware associados à organização
                    var tiposHardware = _tipoHardwareService.GetByIdOrganizacao(id);
                    foreach (var tipoHardware in tiposHardware)
                    {
                        if (!_tipoHardwareService.Remove((int)tipoHardware.Id))
                            throw new ServiceException($"Erro ao remover o tipo de hardware: {tipoHardware.Descricao}");
                    }

                    // Remove a organização
                    var organizacao = _context.Organizacaos.FirstOrDefault(o => o.Id == id);
                    if (organizacao != null)
                    {
                        _context.Remove(organizacao);
                        if (_context.SaveChanges() != 1)
                            throw new ServiceException("Erro ao remover a organização.");
                    }

                    transaction.Commit();
                    return true;
                }
                catch (ServiceException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch (MySqlException ex)
                {
                    transaction.Rollback();
                    if (ex.Message.Contains("Cannot delete or update a parent row: a foreign key constraint fails"))
                    {
                        throw new ServiceException("A exclusão da organização não é possível, pois existem usuários ou blocos associados a ela.");
                    }
                    else
                    {
                        throw new ServiceException("Ocorreu um erro inesperado ao tentar remover a organização.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ServiceException("Erro ao tentar remover a organização: " + ex.Message);
                }
            }
        }


        /// <summary>
        /// Atualiza os dados de uma organização existente.
        /// </summary>
        /// <param name="entity">Modelo da organização com os dados atualizados.</param>
        /// <returns>True se a atualização for bem-sucedida, false caso contrário.</returns>
        /// <exception cref="ServiceException">Lançada se o CNPJ já estiver sendo usado por outra organização.</exception>
        public bool Update(OrganizacaoModel entity)
        {
            try
            {
                var empresa = GetByCnpj(entity.Cnpj);
                if (empresa != null && empresa.Id != entity.Id)
                    throw new ServiceException("A organização não pode ser removida, pois ainda existem usuários ou blocos associados a ela.");

                var x = _context.Organizacaos.Where(o => o.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        /// <summary>
        /// Método auxiliar para mapear um modelo de organização para uma entidade de organização.
        /// </summary>
        /// <param name="model">Modelo de organização de origem.</param>
        /// <param name="entity">Entidade de organização de destino.</param>
        /// <returns>Entidade de organização atualizada.</returns>
        private static Organizacao SetEntity(OrganizacaoModel model, Organizacao entity)
        {
            entity.Id = model.Id;
            entity.RazaoSocial = model.RazaoSocial;
            entity.Cnpj = model.Cnpj;

            return entity;
        }

        /// <summary>
        /// Recupera as organizações associadas a um usuário específico.
        /// </summary>
        /// <param name="idUsuario">Identificador do usuário.</param>
        /// <returns>Lista de organizações associadas ao usuário.</returns>
        public List<OrganizacaoModel> GetByIdUsuario(uint idUsuario)
        {
            var _usuarioOrgService = new UsuarioOrganizacaoService(_context);
            var query = (from uo in _usuarioOrgService.GetByIdUsuario(idUsuario)
                         join org in GetAll() on uo.OrganizacaoId equals org.Id
                         select new OrganizacaoModel
                         {
                             Id = org.Id,
                             Cnpj = org.Cnpj,
                             RazaoSocial = org.RazaoSocial,
                         }).ToList();

            return query;
        }
    }
}
