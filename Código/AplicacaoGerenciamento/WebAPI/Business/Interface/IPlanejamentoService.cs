using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IPlanejamentoService
    {
        List<PlanejamentoModel> GetAll();
        PlanejamentoModel GetById(int id);
        List<PlanejamentoModel> GetByIdSala(int id);
        List<PlanejamentoModel> GetByIdUsuario(int idUsuario);

        List<PlanejamentoModel> GetByIdOrganizacao(int idOrganizacao);
        bool InsertListHorariosPlanjamento(PlanejamentoModel entity);
        bool Insert(PlanejamentoModel entity);
        bool Remove(int id);
        bool Update(PlanejamentoModel entity);
    }
}
