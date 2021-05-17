using Model;
using Model.AuxModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IPlanejamentoService
    {
        List<PlanejamentoModel> GetAll();
        PlanejamentoModel GetById(int id);
        List<PlanejamentoModel> GetByIdSala(int id);
        List<PlanejamentoModel> GetByIdUsuario(int idUsuario);

        List<PlanejamentoModel> GetByIdOrganizacao(int idOrganizacao);
        bool InsertPlanejamentoWithListHorarios(PlanejamentoAuxModel entity);
        bool Insert(PlanejamentoModel entity);
        bool Remove(int id, bool excluirReservas);
        bool RemoveByUsuario(int id);
        bool Update(PlanejamentoModel entity);
    }
}
