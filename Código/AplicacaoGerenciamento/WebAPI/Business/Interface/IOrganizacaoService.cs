using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IOrganizacaoService
    {
        List<OrganizacaoModel> GetAll();
        List<OrganizacaoModel> GetInList(List<int> ids);
        OrganizacaoModel GetById(int id);
        OrganizacaoModel GetByCnpj(string cnpj);
        bool Insert(OrganizacaoModel entity);
        bool Remove(int id);
        bool Update(OrganizacaoModel entity);
    }
}
