using Model;
using Model.ViewModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IEquipamentoService
    {
        EquipamentoModel GetByIdEquipamento(int idEquipamento);
        EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo);
        List<EquipamentoModel> GetByIdSala(uint idSala);
        List<EquipamentoModel> GetAll();
        bool Insert(EquipamentoViewModel entity);
        bool Update(EquipamentoViewModel entity);
        bool Remove(int id);
    }
}
