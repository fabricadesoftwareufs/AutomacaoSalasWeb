using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IMonitoramentoService
    {
        List<MonitoramentoModel> GetAll();
        MonitoramentoModel GetById(int id);
        MonitoramentoModel GetByIdSala(int idSala);
        bool Insert(MonitoramentoModel model);
        bool Update(MonitoramentoModel model);
    }
}
