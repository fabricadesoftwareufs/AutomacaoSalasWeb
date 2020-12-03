using Model;

namespace Service.Interface
{
    public interface IEquipamentoService
    {
        EquipamentoModel GetByIdSalaAndTipoEquipamento(int id, string tipo);
    }
}
