using Model;
using Model.AuxModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ISalaService
    {
        List<SalaModel> GetAll();
        SalaModel GetById(uint id);
        SalaModel GetByTitulo(string titulo);
        List<SalaModel> GetByIdBloco(uint id);

        List<SalaModel> GetAllByIdUsuarioOrganizacao(uint idUsuario);
        bool InsertSalaWithHardwaresOrSalasWithPontosdeAcesso(SalaAuxModel sala, uint idUsuario);
        SalaModel Insert(SalaModel salaModel);
        bool Remove(uint id);
        bool Update(SalaModel entity);
    }
}
