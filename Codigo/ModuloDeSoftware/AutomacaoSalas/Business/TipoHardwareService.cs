using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Serviço responsável pela gestão dos tipos de hardware.
    /// </summary>
    public class TipoHardwareService : ITipoHardwareService
    {   
        private readonly SalasDBContext _context;
        
        /// <summary>
        /// Inicializa uma nova instância do serviço TipoHardwareService.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public TipoHardwareService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os tipos de hardware.
        /// </summary>
        /// <returns>Lista de tipos de hardware.</returns>
        public List<TipoHardwareModel> GetAll() => _context.Tipohardwares.Select(th => new TipoHardwareModel { Id = th.Id, Descricao = th.Descricao }).ToList();

        /// <summary>
        /// Obtém um tipo de hardware pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de hardware.</param>
        /// <returns>Modelo do tipo de hardware correspondente.</returns>
        public TipoHardwareModel GetById(uint id) => _context.Tipohardwares.Where(th => th.Id == id).Select(th => new TipoHardwareModel { Id = th.Id, Descricao = th.Descricao }).FirstOrDefault();

        /// <summary>
        /// Obtém tipos de hardware associados a uma organização.
        /// </summary>
        /// <param name="organizacaoId">ID da organização.</param>
        /// <returns>Lista de tipos de hardware da organização.</returns>
        public List<TipoHardwareModel> GetByIdOrganizacao(uint organizacaoId)
        {
            return _context.Tipohardwares
                .Where(th => th.IdOrganizacao == organizacaoId)
                .Select(th => new TipoHardwareModel
                {
                    Id = th.Id,
                    Descricao = th.Descricao,
                    IdOrganizacao = th.IdOrganizacao
                })
                .ToList();
        }

        /// <summary>
        /// Insere um novo tipo de hardware.
        /// </summary>
        /// <param name="entity">Modelo do tipo de hardware a ser inserido.</param>
        /// <returns>Retorna verdadeiro se o tipo de hardware foi inserido com sucesso.</returns>
        public bool Insert(TipoHardwareModel entity)
        {
            _context.Add(SetEntity(entity, new Tipohardware()));
            return _context.SaveChanges() == 1;
        }

        /// <summary>
        /// Remove um tipo de hardware pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de hardware a ser removido.</param>
        /// <returns>Retorna verdadeiro se o tipo de hardware foi removido com sucesso.</returns>
        public bool Remove(int id)
        {
            var x = _context.Tipohardwares.Where(th => th.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        /// <summary>
        /// Atualiza os dados de um tipo de hardware.
        /// </summary>
        /// <param name="entity">Modelo do tipo de hardware a ser atualizado.</param>
        /// <returns>Retorna verdadeiro se o tipo de hardware foi atualizado com sucesso.</returns>
        public bool Update(TipoHardwareModel entity)
        {
            var x = _context.Tipohardwares.Where(th => th.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        /// <summary>
        /// Configura uma entidade TipoHardware a partir de um modelo.
        /// </summary>
        /// <param name="model">Modelo do tipo de hardware.</param>
        /// <param name="entity">Entidade do tipo de hardware.</param>
        /// <returns>Entidade configurada.</returns>
        private static Tipohardware SetEntity(TipoHardwareModel model, Tipohardware entity)
        {
            entity.Id = model.Id;
            entity.Descricao = model.Descricao;

            return entity;
        }
    }
}
