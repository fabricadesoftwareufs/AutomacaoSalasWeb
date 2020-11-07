using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ICodigoInfravermelhoService
    {
        CodigoInfravermelhoModel GetById(int id);

        List<CodigoInfravermelhoModel> GetByIdOperacaoAndIdEquipamento(int idEquipamento, int idOperacao);

    }
}
