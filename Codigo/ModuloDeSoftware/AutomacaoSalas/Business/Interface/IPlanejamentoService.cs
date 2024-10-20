using Model;
using Model.AuxModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IPlanejamentoService
    {
        List<PlanejamentoModel> GetAll();
        PlanejamentoModel GetById(int id);
        List<PlanejamentoModel> GetByIdSala(uint id);
        List<PlanejamentoModel> GetByIdUsuario(uint idUsuario);

        List<PlanejamentoModel> GetByIdOrganizacao(uint idOrganizacao);
        bool InsertPlanejamentoWithListHorarios(PlanejamentoAuxModel entity);
        bool Insert(PlanejamentoModel entity);
        bool Remove(int id, bool excluirReservas);
        bool RemoveByUsuario(uint id);
        bool Update(PlanejamentoModel entity);
    }
}
