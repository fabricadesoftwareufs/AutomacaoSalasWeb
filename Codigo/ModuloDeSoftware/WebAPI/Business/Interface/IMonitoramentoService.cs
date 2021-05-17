using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IMonitoramentoService
    {
        List<MonitoramentoModel> GetAll();
        MonitoramentoModel GetById(int id);
        MonitoramentoModel GetByIdSala(int idSala);
        bool Insert(MonitoramentoModel model);
        bool Update(MonitoramentoModel model);
        bool MonitorarSala(int idUsuario, MonitoramentoModel model);

    }
}
