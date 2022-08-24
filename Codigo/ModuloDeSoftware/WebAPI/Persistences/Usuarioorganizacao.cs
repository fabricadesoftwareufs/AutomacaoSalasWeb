using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Usuarioorganizacao
    {
        public int Id { get; set; }
        public int Organizacao { get; set; }
        public int Usuario { get; set; }

        public virtual Organizacao OrganizacaoNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
    }
}
