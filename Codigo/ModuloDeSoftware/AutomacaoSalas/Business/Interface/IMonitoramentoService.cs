using Model;
using Model.ViewModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IMonitoramentoService
    {
        List<MonitoramentoModel> GetAll();
        MonitoramentoModel GetById(int id);
        MonitoramentoModel GetByIdEquipamento(int idEquipamento);
        bool Insert(MonitoramentoModel model);
        bool Update(MonitoramentoModel model);
        List<MonitoramentoModel> GetByIdSala(uint idSala);
        bool MonitorarSala(uint idUsuario, MonitoramentoViewModel monitoramento);
        bool MonitorarEquipamento(uint idUsuario, MonitoramentoModel model);
        MonitoramentoModel GetByIdSalaAndTipoEquipamento(uint idSala, string tipoEquipamento);

    }
}
