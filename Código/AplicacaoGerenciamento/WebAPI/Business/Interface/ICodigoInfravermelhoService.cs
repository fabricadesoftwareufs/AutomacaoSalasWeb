using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ICodigoInfravermelhoService
    {
        CodigoInfravermelhoModel GetById(int id);

        List<CodigoInfravermelhoModel> GetByIdOperacaoAndIdEquipamento(int idEquipamento, int idOperacao);

    }
}
