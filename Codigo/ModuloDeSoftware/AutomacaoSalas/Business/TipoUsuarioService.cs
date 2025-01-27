using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Serviço responsável por gerenciar as operações relacionadas aos tipos de usuários.
    /// </summary>
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Construtor da classe TipoUsuarioService.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public TipoUsuarioService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os tipos de usuários.
        /// </summary>
        /// <returns>Lista de todos os tipos de usuários.</returns>
        public List<TipoUsuarioModel> GetAll() => _context.Tipousuarios
            .Select(tu => new TipoUsuarioModel { Id = tu.Id, Descricao = tu.Descricao })
            .ToList();

        /// <summary>
        /// Obtém o tipo de usuário com base no ID do usuário.
        /// </summary>
        /// <param name="idUsuario">ID do usuário.</param>
        /// <returns>Tipo de usuário correspondente ou null se não encontrado.</returns>
        public TipoUsuarioModel GetTipoUsuarioByUsuarioId(uint idUsuario)
        {
            // Busca o tipo de usuário a partir da tabela Usuarioorganizacao
            return _context.Usuarioorganizacaos
                .Where(uo => uo.IdUsuario == idUsuario)
                .Select(uo => new TipoUsuarioModel
                {
                    Id = uo.IdTipoUsuarioNavigation.Id,
                    Descricao = uo.IdTipoUsuarioNavigation.Descricao
                })
                .FirstOrDefault();
        }

        /// <summary>
        /// Insere um novo tipo de usuário no banco de dados.
        /// </summary>
        /// <param name="entity">Modelo do tipo de usuário a ser inserido.</param>
        /// <returns>True se a inserção for bem-sucedida, caso contrário, false.</returns>
        public bool Insert(TipoUsuarioModel entity)
        {
            _context.Add(SetEntity(entity, new Tipousuario()));
            return _context.SaveChanges() == 1;
        }

        /// <summary>
        /// Remove um tipo de usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de usuário a ser removido.</param>
        /// <returns>True se a remoção for bem-sucedida, caso contrário, false.</returns>
        public bool Remove(int id)
        {
            var x = _context.Tipousuarios.FirstOrDefault(tu => tu.Id == id);
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1;
            }

            return false;
        }

        /// <summary>
        /// Atualiza os dados de um tipo de usuário existente.
        /// </summary>
        /// <param name="entity">Modelo do tipo de usuário com os novos dados.</param>
        /// <returns>True se a atualização for bem-sucedida, caso contrário, false.</returns>
        public bool Update(TipoUsuarioModel entity)
        {
            var x = _context.Tipousuarios.FirstOrDefault(tu => tu.Id == entity.Id);
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1;
            }

            return false;
        }

        /// <summary>
        /// Mapeia os dados de um modelo para uma entidade.
        /// </summary>
        /// <param name="model">Modelo com os dados.</param>
        /// <param name="entity">Entidade a ser preenchida.</param>
        /// <returns>Entidade preenchida com os dados do modelo.</returns>
        private static Tipousuario SetEntity(TipoUsuarioModel model, Tipousuario entity)
        {
            entity.Id = model.Id;
            entity.Descricao = model.Descricao;

            return entity;
        }
    }
}