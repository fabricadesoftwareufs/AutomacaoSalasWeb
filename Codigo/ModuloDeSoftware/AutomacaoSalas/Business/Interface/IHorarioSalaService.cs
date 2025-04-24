using Model;
using System;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IHorarioSalaService
    {
        List<HorarioSalaModel> GetAll();
        HorarioSalaModel GetById(uint id);
        List<HorarioSalaModel> GetByIdSala(uint id);
        HorarioSalaModel VerificaSalaOcupada(uint idSala, DateTime data, TimeSpan horarioInicio, TimeSpan horarioFim);
        List<HorarioSalaModel> GetByIdUsuarioAndDiaSemana(uint idUsuario, string diaSemana);
        List<HorarioSalaModel> GetByIdUsuario(uint idUsuario);
        IEnumerable<HorarioSalaModel> GetProximasReservasByIdUsuarioAndDiaSemana(uint idUsuario, string diaSemana);
        List<HorarioSalaModel> GetReservasDaSemanaByIdSala(uint idSala);
        List<HorarioSalaModel> GetReservasDeHojeByIdSala(uint idSala);
        List<HorarioSalaModel> GetReservasDeHojeByUuid(string uuid);
        bool VerificaSeEstaEmHorarioAula(uint idUsuario, uint idSala);
        bool UpdateColumnPlanejamentoForNull(int idPlanejamento);
        bool RemoveByIdPlanejamento(int idPlanejamento);
        bool Insert(HorarioSalaModel entity);
        bool Remove(int id);
        bool RemoveByUsuario(uint id);
        bool ConcelarReserva(uint idReserva);
        bool AprovarReserva(uint idReserva);
        bool Update(HorarioSalaModel entity);
    }
}
