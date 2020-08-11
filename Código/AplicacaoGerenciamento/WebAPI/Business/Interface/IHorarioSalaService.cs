using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IHorarioSalaService
    {
        List<HorarioSalaModel> GetAll();

        HorarioSalaModel GetById(int id);

        List<HorarioSalaModel> GetByIdSala(int id);

        bool Insert(HorarioSalaModel entity);
        bool Remove(int id);

        bool Update(HorarioSalaModel entity);
    }
}
