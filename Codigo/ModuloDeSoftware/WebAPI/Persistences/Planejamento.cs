using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Planejamento
    {
        public Planejamento()
        {
            Horariosalas = new HashSet<Horariosala>();
        }

        public uint Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string DiaSemana { get; set; }
        public string Objetivo { get; set; }
        public uint Usuario { get; set; }
        public uint Sala { get; set; }

        public virtual Sala SalaNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
        public virtual ICollection<Horariosala> Horariosalas { get; set; }
    }
}
