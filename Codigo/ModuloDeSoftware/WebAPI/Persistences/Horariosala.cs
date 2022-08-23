using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Horariosala
    {
        public int Id { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string Situacao { get; set; }
        public string Objetivo { get; set; }
        public int Usuario { get; set; }
        public int Sala { get; set; }
        public int? Planejamento { get; set; }

        public virtual Planejamento PlanejamentoNavigation { get; set; }
        public virtual Sala SalaNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
    }
}
