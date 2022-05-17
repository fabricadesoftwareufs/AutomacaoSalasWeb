using Model;
using System;
using System.Collections.Generic;

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
        List<HorarioSalaModel> GetReservasDaSemanaByIdSala(int idSala);
        List<HorarioSalaModel> GetReservasDeHojeByIdSala(int idSala);
        List<HorarioSalaModel> GetReservasDeHojeByUuid(string uuid);
        bool VerificaSeEstaEmHorarioAula(int idUsuario, int idSala);
        bool UpdateColumnPlanejamentoForNull(int idPlanejamento);
        bool RemoveByIdPlanejamento(int idPlanejamento);
        bool Insert(HorarioSalaModel entity);
        bool Remove(int id);
        bool RemoveByUsuario(int id);
        bool ConcelarReserva(int idReserva);
        bool Update(HorarioSalaModel entity);
        bool SolicitaAtualizacaoHorarioESP(string ipSala, DateTime dataHorario);
    }
}
