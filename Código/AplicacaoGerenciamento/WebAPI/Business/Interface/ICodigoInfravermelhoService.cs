using Model;

namespace Service.Interface
{
    public interface ICodigoInfravermelhoService
    {
        CodigoInfravermelhoModel GetByIdSalaAndIdOperacao(int idSala, int operacao);
        CodigoInfravermelhoModel GetById(int id);
        CodigoInfravermelhoModel GetByIdOperacaoAndIdEquipamento(int idEquipamento, int idOperacao);

    }
}
