using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Monitoramento
{
    public int Id { get; set; }

    public bool Luzes { get; set; }

    public bool ArCondicionado { get; set; }

    public uint Sala { get; set; }

    public virtual Sala SalaNavigation { get; set; } = null!;
}
