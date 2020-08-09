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
        List<SalaModel> GetByIdBloco(int id);
        bool InsertSalaWithHardwares(SalaViewModel sala);
        SalaModel Insert(SalaModel salaModel);
        bool Remove(int id);
        bool Update(SalaModel entity);
        List<SalaModel> GetSelectedList();
    }
}
