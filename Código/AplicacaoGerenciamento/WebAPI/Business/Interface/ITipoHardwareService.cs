using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ITipoHardwareService
    {
        List<TipoHardwareModel> GetAll();
        TipoHardwareModel GetById(int id);

        bool Insert(TipoHardwareModel entity);

        bool Remove(int id);
        bool Update(TipoHardwareModel entity);
    }
}
