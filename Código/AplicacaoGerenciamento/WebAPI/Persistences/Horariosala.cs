using System;

namespace Persistences
{
    public partial class Horariosala
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public string Turno { get; set; }
        public int QtdAlunos { get; set; }
        public int Usuario { get; set; }
        public int? Disciplina { get; set; }
        public int Sala { get; set; }

        public Disciplina DisciplinaNavigation { get; set; }
        public Sala SalaNavigation { get; set; }
        public Usuario UsuarioNavigation { get; set; }
    }
}
