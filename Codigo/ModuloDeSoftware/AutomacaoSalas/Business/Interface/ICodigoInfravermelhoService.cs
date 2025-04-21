using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ICodigoInfravermelhoService
    {
        CodigoInfravermelhoModel GetByIdSalaAndIdOperacao(int idSala, int operacao);
        CodigoInfravermelhoModel GetById(int id);
        CodigoInfravermelhoModel GetByIdOperacaoAndIdModeloEquipamento(int idModeloEquipamento, int idOperacao);
        List<CodigoInfravermelhoModel> GetAllByEquipamento(int idEquipamento);
        bool AddAll(List<CodigoInfravermelhoModel> codigoInfravermelhoModels);
        bool UpdateAll(List<CodigoInfravermelhoModel> codigoInfravermelhoModels);
        bool RemoveAll(List<CodigoInfravermelhoModel> codigoInfravermelhoModels);
        List<CodigoInfravermelhoModel> GetAllByUuidHardware(string uuid);
        CodigoInfravermelhoModel Insert(CodigoInfravermelhoModel entity);
    }
}
