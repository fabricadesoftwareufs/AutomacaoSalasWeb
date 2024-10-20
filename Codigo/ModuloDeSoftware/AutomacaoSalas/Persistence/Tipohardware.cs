using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Tipohardware
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Hardwaredesala> Hardwaredesalas { get; set; } = new List<Hardwaredesala>();
}
