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
        HorarioSalaModel VerificaSalaOcupada(int idSala, DateTime data, TimeSpan horarioInicio, TimeSpan horarioFim);
        List<HorarioSalaModel> GetByIdUsuarioAndDiaSemana(int idUsuario, string diaSemana);
        List<HorarioSalaModel> GetByIdUsuario(int idUsuario);
        List<HorarioSalaModel> GetProximasReservasByIdUsuarioAndDiaSemana(int idUsuario, string diaSemana);
        bool Insert(HorarioSalaModel entity);
        bool Remove(int id);
        bool ConcelarReserva(int idReserva);
        bool Update(HorarioSalaModel entity);
    }
}
