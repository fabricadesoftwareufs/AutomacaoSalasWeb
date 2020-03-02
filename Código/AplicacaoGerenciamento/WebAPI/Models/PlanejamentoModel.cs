using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class PlanejamentoModel
    {
        public int Id { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string DiaSemana { get; set; }
        public string Objetivo { get; set; }
        public int SalaId { get; set; }
        public int UsuarioId { get; set; }
    }
}
