using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class HorarioSalaModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public string Turno { get; set; }
        public int QtdAlunos { get; set; }
        public int UsuarioId { get; set; }
        public int DisciplinaId { get; set; }
        public int SalaId { get; set; }
    }
}
