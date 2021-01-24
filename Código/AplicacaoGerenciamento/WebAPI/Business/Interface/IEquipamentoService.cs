using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IEquipamentoService
    {
        EquipamentoModel GetByIdEquipamento(int idEquipamento);
        EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo);
        List<EquipamentoModel> GetByIdSala(int idSala);
        List<EquipamentoModel> GetAll();
    }
}
