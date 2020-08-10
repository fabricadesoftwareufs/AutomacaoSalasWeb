using Model;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IBlocoService
    {
        List<BlocoModel> GetAll();
        BlocoModel GetById(int id);
        List<BlocoModel> GetByIdOrganizacao(int id);
        BlocoModel GetByTitulo(string titulo, int idOrganizacao);
        bool InsertBlocoWithHardware(BlocoModel entity);
        BlocoModel Insert(BlocoModel entity);
        bool Remove(int id);
        bool Update(BlocoModel entity);
        List<BlocoModel> GetSelectedList();
    }
}
