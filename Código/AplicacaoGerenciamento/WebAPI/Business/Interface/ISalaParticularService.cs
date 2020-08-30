using Model;
using Model.AuxModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ISalaParticularService
    {
        List<SalaParticularModel> GetAll();
        SalaParticularModel GetById(int id);
        List<SalaParticularModel> GetByIdSala(int id);
        List<SalaParticularModel> GetByIdUsuario(int id);

        SalaParticularModel GetByIdUsuarioAndIdSala(int idUsuario, int idSala);

        List<SalaParticularModel> GetByIdOrganizacao(int idOrganizacao);

        bool InsertListSalasParticulares(SalaParticularAuxModel entity);
        bool Insert(SalaParticularModel entity);
        bool VerificaSalaExclusivaExistente(int? idSalaExclusiva, int idUsuario, int idSala);
        bool Remove(int id);
        bool RemoveByUsuario(int id);
        bool Update(SalaParticularModel entity);
    }
}
