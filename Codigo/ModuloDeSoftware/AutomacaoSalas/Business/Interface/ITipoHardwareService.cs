using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ITipoHardwareService
    {
        List<TipoHardwareModel> GetAll();
        List<TipoHardwareModel> GetByIdOrganizacao(uint organizacaoId);
        TipoHardwareModel GetById(uint id);

        bool Insert(TipoHardwareModel entity);

        bool Remove(int id);
        bool Update(TipoHardwareModel entity);
    }
}
