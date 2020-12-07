using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ICodigoInfravermelhoService
    {
        CodigoInfravermelhoModel GetById(int id);
        CodigoInfravermelhoModel GetByIdSala(int idSala);

        List<CodigoInfravermelhoModel> GetByIdOperacaoAndIdEquipamento(int idEquipamento, int idOperacao);

    }
}
