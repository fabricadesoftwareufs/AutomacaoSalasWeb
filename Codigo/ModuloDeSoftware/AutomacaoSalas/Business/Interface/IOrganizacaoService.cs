using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IOrganizacaoService
    {
        List<OrganizacaoModel> GetAll();
        List<OrganizacaoModel> GetInList(List<uint> ids);
        OrganizacaoModel GetById(uint id);
        List<OrganizacaoModel> GetByIdUsuario(uint idUsuario);
        OrganizacaoModel GetByCnpj(string cnpj);
        bool Insert(OrganizacaoModel entity);
        bool Remove(uint id);
        bool Update(OrganizacaoModel entity);
    }
}
