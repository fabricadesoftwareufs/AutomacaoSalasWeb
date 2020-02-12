using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class PlanejamentoModel
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
    }
}
