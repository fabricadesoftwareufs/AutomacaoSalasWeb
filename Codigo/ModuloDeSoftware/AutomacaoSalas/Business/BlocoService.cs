using Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Persistence;

namespace Service
{
    /// <summary>
    /// Serviço responsável pela gestão de blocos.
    /// </summary>
    public class BlocoService : IBlocoService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Inicializa uma nova instância do serviço BlocoService.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public BlocoService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os blocos.
        /// </summary>
        /// <returns>Lista de blocos.</returns>
        public List<BlocoModel> GetAll() => _context.Blocos.Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.IdOrganizacao, Titulo = b.Titulo }).ToList();

        /// <summary>
        /// Obtém um bloco pelo ID.
        /// </summary>
        /// <param name="id">ID do bloco.</param>
        /// <returns>Modelo do bloco correspondente.</returns>
        public BlocoModel GetById(uint id)
        {
            var blocoModel = (from b in _context.Blocos
                              join o in _context.Organizacaos on b.IdOrganizacao equals o.Id
                              where b.Id == id
                              select new BlocoModel
                              {
                                  Id = b.Id,
                                  OrganizacaoId = b.IdOrganizacao,
                                  Titulo = b.Titulo,
                                  NomeOrganizacao = o.RazaoSocial
                              }).FirstOrDefault();

            return blocoModel;
        }

        /// <summary>
        /// Obtém blocos pelo ID da organização.
        /// </summary>
        /// <param name="id">ID da organização.</param>
        /// <returns>Lista de blocos da organização.</returns>
        public List<BlocoModel> GetByIdOrganizacao(uint id) => _context.Blocos.Where(b => b.IdOrganizacao == id).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.IdOrganizacao, Titulo = b.Titulo }).ToList();

        /// <summary>
        /// Obtém um bloco pelo título e ID da organização.
        /// </summary>
        /// <param name="titulo">Título do bloco.</param>
        /// <param name="idOrganizacao">ID da organização.</param>
        /// <returns>Modelo do bloco correspondente.</returns>
        public BlocoModel GetByTitulo(string titulo, uint idOrganizacao) => _context.Blocos.Where(b => b.Titulo.ToUpper().Equals(titulo.ToUpper()) && b.IdOrganizacao == idOrganizacao).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.IdOrganizacao, Titulo = b.Titulo }).FirstOrDefault();

        /// <summary>
        /// Insere um bloco e associa com hardware.
        /// </summary>
        /// <param name="blocoModel">Modelo do bloco a ser inserido.</param>
        /// <param name="idUsuario">ID do usuário responsável pela inserção.</param>
        /// <returns>Retorna verdadeiro se o bloco foi inserido com sucesso.</returns>
        public bool InsertBlocoWithHardware(BlocoModel blocoModel, uint idUsuario)
        {
            var blocoInserido = new BlocoModel();
            try
            {
                blocoInserido = Insert(new BlocoModel { Id = blocoModel.Id, OrganizacaoId = blocoModel.OrganizacaoId, Titulo = blocoModel.Titulo });
                if (blocoInserido == null) throw new ServiceException("Houve um problema ao cadastrar bloco, tente novamente em alguns minutos!");
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        /// <summary>
        /// Insere um novo bloco.
        /// </summary>
        /// <param name="blocoModel">Modelo do bloco a ser inserido.</param>
        /// <returns>Bloco inserido.</returns>
        public BlocoModel Insert(BlocoModel blocoModel)
        {
            try
            {
                if (GetByTitulo(blocoModel.Titulo, blocoModel.OrganizacaoId) != null)
                    throw new ServiceException("Essa organização já possui um bloco com esse nome");

                var entity = new Bloco();
                _context.Add(SetEntity(blocoModel, entity));
                var save = _context.SaveChanges() == 1 ? true : false;

                if (save)
                {
                    blocoModel.Id = entity.Id; return blocoModel;
                }
                else return null;
            }
            catch (Exception e) { throw e; }
        }

        /// <summary>
        /// Remove um bloco pelo ID.
        /// </summary>
        /// <param name="id">ID do bloco a ser removido.</param>
        /// <returns>Retorna verdadeiro se o bloco foi removido com sucesso.</returns>
        public bool Remove(uint id)
        {
            var _salaService = new SalaService(_context);
            try
            {
                if (_salaService.GetByIdBloco(id).Count == 0)
                {
                    var x = _context.Blocos.Where(b => b.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        return _context.SaveChanges() == 1;
                    }
                }
                else throw new ServiceException("Esse Bloco não pode ser removido pois possui hardwares e salas associados a ele!");

            }
            catch (Exception e) { throw e; }

            return false;
        }

        /// <summary>
        /// Atualiza os dados de um bloco.
        /// </summary>
        /// <param name="entity">Modelo do bloco a ser atualizado.</param>
        /// <returns>Retorna verdadeiro se o bloco foi atualizado com sucesso.</returns>
        public bool Update(BlocoModel entity)
        {
            try
            {
                var bloco = GetByTitulo(entity.Titulo, entity.OrganizacaoId);
                if (bloco != null && bloco.Id != entity.Id)
                    throw new ServiceException("Essa organização já possui um bloco com esse nome");

                var x = _context.Blocos.Where(b => b.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e) { throw e; }

            return false;
        }

        /// <summary>
        /// Configura uma entidade Bloco a partir de um modelo.
        /// </summary>
        /// <param name="model">Modelo do bloco.</param>
        /// <param name="entity">Entidade do bloco.</param>
        /// <returns>Entidade configurada.</returns>
        private static Bloco SetEntity(BlocoModel model, Bloco entity)
        {
            entity.Id = model.Id;
            entity.IdOrganizacao = model.OrganizacaoId;
            entity.Titulo = model.Titulo;

            return entity;
        }

        /// <summary>
        /// Obtém todos os blocos de uma organização associada a um usuário.
        /// </summary>
        /// <param name="idUsuario">ID do usuário.</param>
        /// <returns>Lista de blocos.</returns>
        public List<BlocoModel> GetAllByIdUsuarioOrganizacao(uint idUsuario)
        {
            var queryUser = from usuario in _context.Usuarioorganizacaos
                            where usuario.IdUsuario == idUsuario
                            select usuario;
            var usuarioorganizacao = queryUser.FirstOrDefault();

            var query = from bloco in _context.Blocos
                        where bloco.IdOrganizacao == usuarioorganizacao.IdOrganizacao
                        select new BlocoModel
                        {
                            Id = bloco.Id,
                            Titulo = bloco.Titulo,
                            OrganizacaoId = bloco.IdOrganizacao

                        };
            return query.ToList();
        }
    }
}
