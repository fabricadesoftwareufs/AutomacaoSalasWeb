using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IOrganizacaoService
    {
        List<OrganizacaoModel> GetAll();
        List<OrganizacaoModel> GetInList(List<int> ids);
        OrganizacaoModel GetById(int id);
        List<OrganizacaoModel> GetByIdUsuario(int idUsuario);
        OrganizacaoModel GetByCnpj(string cnpj);
        bool Insert(OrganizacaoModel entity);
        bool Remove(int id);
        bool Update(OrganizacaoModel entity);
    }
}
