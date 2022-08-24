using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Usuarioorganizacao
    {
        public uint Id { get; set; }
        public uint Organizacao { get; set; }
        public uint Usuario { get; set; }

        public virtual Organizacao OrganizacaoNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
    }
}
