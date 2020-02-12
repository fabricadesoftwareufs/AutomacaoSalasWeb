using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Planejamento
    {
        public int IdPlanejamento { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public TimeSpan HoarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public int SalaId { get; set; }
        public int UsuarioId { get; set; }
        public string DiaSemana { get; set; }
        public string Objetivo { get; set; }

        public Sala Sala { get; set; }
        public Usuario Usuario { get; set; }
    }
}
