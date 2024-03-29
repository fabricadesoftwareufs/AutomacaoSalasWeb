﻿using System;
using System.Collections.Generic;



namespace Persistence
{
    public partial class Planejamento
    {
        public Planejamento()
        {
            Horariosala = new HashSet<Horariosala>();
        }

        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string DiaSemana { get; set; }
        public string Objetivo { get; set; }
        public int Usuario { get; set; }
        public int Sala { get; set; }

        public virtual Sala SalaNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
        public virtual ICollection<Horariosala> Horariosala { get; set; }
    }
}
