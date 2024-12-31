using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Salaparticular
{
    public uint Id { get; set; }

    public uint IdUsuario { get; set; }

    public uint IdSala { get; set; }

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
