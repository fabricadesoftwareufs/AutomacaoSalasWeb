using Model;
using Model.AuxModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ISalaParticularService
    {
        List<SalaParticularModel> GetAll();
        SalaParticularModel GetById(uint id);
        List<SalaParticularModel> GetByIdSala(uint id);
        List<SalaParticularModel> GetByIdUsuario(uint id);

        SalaParticularModel GetByIdUsuarioAndIdSala(uint idUsuario, uint idSala);

        List<SalaParticularModel> GetByIdOrganizacao(uint idOrganizacao);

        bool InsertListSalasParticulares(SalaParticularAuxModel entity);
        bool Insert(SalaParticularModel entity);
        bool VerificaSalaExclusivaExistente(uint? idSalaExclusiva, uint idUsuario, uint idSala);
        bool Remove(uint id);
        bool RemoveByUsuario(uint id);
        bool Update(SalaParticularModel entity);
    }
}
