using Model;
using MySql.Data.MySqlClient;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OrganizacaoService : IOrganizacaoService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Inicializa uma nova instância da classe OrganizacaoService.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        public OrganizacaoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recupera todas as organizações.
        /// </summary>
        /// <returns>Uma lista com todas as organizações.</returns>
        public List<OrganizacaoModel> GetAll()
            => _context.Organizacaos.Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).ToList();

        /// <summary>
        /// Recupera uma lista de organizações pelos seus IDs.
        /// </summary>
        /// <param name="ids">Uma lista com os IDs das organizações.</param>
        /// <returns>Uma lista de organizações que correspondem aos IDs fornecidos.</returns>
        public List<OrganizacaoModel> GetInList(List<uint> ids)
            => _context.Organizacaos.Where(o => ids.Contains(o.Id)).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).ToList();

        /// <summary>
        /// Recupera uma organização pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da organização.</param>
        /// <returns>A organização com o ID fornecido ou null se não for encontrada.</returns>
        public OrganizacaoModel GetById(uint id)
            => _context.Organizacaos.Where(o => o.Id == id).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).FirstOrDefault();

        /// <summary>
        /// Recupera uma organização pelo seu CNPJ.
        /// </summary>
        /// <param name="cnpj">O CNPJ da organização.</param>
        /// <returns>A organização com o CNPJ fornecido ou null se não for encontrada.</returns>
        public OrganizacaoModel GetByCnpj(string cnpj)
            => _context.Organizacaos.Where(o => o.Cnpj.Equals(cnpj)).Select(o => new OrganizacaoModel { Id = o.Id, Cnpj = o.Cnpj, RazaoSocial = o.RazaoSocial }).FirstOrDefault();

        /// <summary>
        /// Insere uma nova organização no banco de dados.
        /// </summary>
        /// <param name="entity">A organização a ser inserida.</param>
        /// <returns>Retorna verdadeiro se a organização foi inserida com sucesso, caso contrário, falso.</returns>
        public bool Insert(OrganizacaoModel entity)
        {
            try
            {
                if (GetByCnpj(entity.Cnpj) != null)
                    throw new ServiceException("Uma organização com esse cnpj já está cadastrada!");

                _context.Add(SetEntity(entity, new Organizacao()));
                return _context.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Remove uma organização pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da organização a ser removida.</param>
        /// <returns>Retorna verdadeiro se a organização foi removida com sucesso, caso contrário, falso.</returns>
        public bool Remove(uint id)
        {
            var _blocoService = new BlocoService(_context);
            var _usuarioOrganizacaoService = new UsuarioOrganizacaoService(_context);
            try
            {
                var blocos = _blocoService.GetByIdOrganizacao(id);
                var usuarios = _usuarioOrganizacaoService.GetByIdOrganizacao(id);

                if (blocos.Count > 0 || usuarios.Count > 0)
                {
                    throw new ServiceException("Organização não pode ser removida pois ainda existem usuários ou blocos associados a ela!");
                }

                var organizacao = _context.Organizacaos.FirstOrDefault(o => o.Id == id);
                if (organizacao != null)
                {
                    try
                    {
                        _context.Remove(organizacao);
                        _context.SaveChanges();
                        return true;
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Message.Contains("Cannot delete or update a parent row: a foreign key constraint fails"))
                        {
                            throw new ServiceException("Não é possível excluir a organização pois existem usuários ou blocos associados a ela.");
                        }
                        else
                        {
                            throw new ServiceException("Ocorreu um erro inesperado ao tentar remover a organização.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("Erro ao tentar remover a organização: " + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Atualiza uma organização existente.
        /// </summary>
        /// <param name="entity">Os dados da organização a serem atualizados.</param>
        /// <returns>Retorna verdadeiro se a organização foi atualizada com sucesso, caso contrário, falso.</returns>
        public bool Update(OrganizacaoModel entity)
        {
            try
            {
                var empresa = GetByCnpj(entity.Cnpj);
                if (empresa != null && empresa.Id != entity.Id)
                    throw new ServiceException("Organização não pode ser removida pois ainda existem usuários ou blocos associados a ela!");

                var x = _context.Organizacaos.Where(o => o.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        /// <summary>
        /// Mapeia um modelo OrganizacaoModel para uma entidade Organizacao.
        /// </summary>
        /// <param name="model">O modelo a ser mapeado.</param>
        /// <param name="entity">A entidade existente a ser atualizada.</param>
        /// <returns>A entidade mapeada.</returns>
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
        /// <param name="idUsuario">O ID do usuário.</param>
        /// <returns>Uma lista de organizações associadas ao usuário informado.</returns>
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