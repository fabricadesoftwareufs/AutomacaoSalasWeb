﻿using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Monitoramento
{
    public int Id { get; set; }

    public sbyte Estado { get; set; }

    public int Equipamento { get; set; }

    public virtual Equipamento EquipamentoNavigation { get; set; } = null!;
}
