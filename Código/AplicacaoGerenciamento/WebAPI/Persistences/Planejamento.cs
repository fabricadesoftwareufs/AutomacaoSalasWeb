using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Planejamento
    {
        public int Id { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string DiaSemana { get; set; }
        public string Objetivo { get; set; }
        public int Usuario { get; set; }
        public int Sala { get; set; }

        public Sala SalaNavigation { get; set; }
        public Usuario UsuarioNavigation { get; set; }
    }
}
