using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Salaparticular
    {
        public int Id { get; set; }
        public int Usuario { get; set; }
        public int Sala { get; set; }

        public virtual Sala SalaNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
    }
}
