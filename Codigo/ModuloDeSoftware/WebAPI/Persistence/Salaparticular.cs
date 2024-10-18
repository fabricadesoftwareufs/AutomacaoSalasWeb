using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Salaparticular
{
    public uint Id { get; set; }

    public uint Usuario { get; set; }

    public uint Sala { get; set; }

    public virtual Sala SalaNavigation { get; set; } = null!;

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
