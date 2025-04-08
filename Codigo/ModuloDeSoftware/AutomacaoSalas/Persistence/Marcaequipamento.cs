using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Marcaequipamento
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Modeloequipamento> Modeloequipamentos { get; set; } = new List<Modeloequipamento>();
}
