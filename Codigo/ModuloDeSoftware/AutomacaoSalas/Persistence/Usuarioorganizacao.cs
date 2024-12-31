using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Usuarioorganizacao
{
    public uint IdOrganizacao { get; set; }

    public uint IdUsuario { get; set; }

    public uint IdTipoUsuario { get; set; }

    public DateTime DataCadastro { get; set; }

    public virtual Organizacao IdOrganizacaoNavigation { get; set; } = null!;

    public virtual Tipousuario IdTipoUsuarioNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
