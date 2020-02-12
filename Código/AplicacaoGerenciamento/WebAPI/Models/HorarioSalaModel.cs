using System;

namespace Models
{
    public class HorarioSalaModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int UsuarioId { get; set; }
        public int SalaId { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string Situacao { get; set; }
        public string Objetivo { get; set; }

    }
}
