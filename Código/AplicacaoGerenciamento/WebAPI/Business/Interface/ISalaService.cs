using Model;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ISalaService
    {
        List<SalaModel> GetAll();
        SalaModel GetById(int id);
        SalaModel GetByTitulo(string titulo);
        List<SalaModel> GetByIdBloco(int id);

        List<SalaModel> GetAllByIdUsuarioOrganizacao(int idUsuario);
        bool InsertSalaWithHardwares(SalaModel sala, int idUsuario);
        SalaModel Insert(SalaModel salaModel);
        bool Remove(int id);
        bool Update(SalaModel entity);
    }
}
