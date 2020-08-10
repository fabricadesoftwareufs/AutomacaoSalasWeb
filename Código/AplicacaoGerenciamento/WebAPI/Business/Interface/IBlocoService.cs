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
        List<BlocoModel> GetAllByIdUsuarioOrganizacao(int idUsuario);
        BlocoModel GetByTitulo(string titulo, int idOrganizacao);
        bool InsertBlocoWithHardware(BlocoModel entity,int idUsuario);
        BlocoModel Insert(BlocoModel entity);
        bool Remove(int id);
        bool Update(BlocoModel entity);
    }
}
