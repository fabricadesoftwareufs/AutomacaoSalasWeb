using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Monitoramento
{
    public int Id { get; set; }

    public sbyte Estado { get; set; }

    public int IdEquipamento { get; set; }

    public virtual Equipamento IdEquipamentoNavigation { get; set; } = null!;
}
