using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Usuarioorganizacao
{
    public uint Id { get; set; }

    public uint Organizacao { get; set; }

    public uint Usuario { get; set; }

    public virtual Organizacao OrganizacaoNavigation { get; set; } = null!;

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
