using System;
using System.Collections.Generic;

namespace Persistences
{
    public partial class Minhasala
    {
        public int IdMinhaSala { get; set; }
        public int UsuarioId { get; set; }
        public int SalaId { get; set; }

        public Sala Sala { get; set; }
        public Usuario Usuario { get; set; }
    }
}
