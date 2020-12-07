using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IEquipamentoService
    {
        EquipamentoModel GetByIdSalaAndTipoEquipamento(int id, string tipo);
        List<EquipamentoModel> GetByIdSala(int idSala);
    }
}
