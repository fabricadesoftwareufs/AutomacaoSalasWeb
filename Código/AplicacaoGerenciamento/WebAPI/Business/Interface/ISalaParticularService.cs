using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ISalaParticularService
    {
        List<SalaParticularModel> GetAll();
        SalaParticularModel GetById(int id);
        List<SalaParticularModel> GetByIdSala(int id);

        SalaParticularModel GetByIdUsuarioAndIdSala(int idUsuario, int idSala);

        bool InsertListSalasParticulares(SalaParticularViewModel entity);
        bool Insert(SalaParticularModel entity);
        bool VerificaSalaExclusivaExistente(int? idSalaExclusiva, int idUsuario, int idSala);
        bool Remove(int id);
        bool Update(SalaParticularModel entity);
    }
}
