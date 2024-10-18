using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Tipousuario
{
    public uint Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
