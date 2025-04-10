using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Equipamento
{
    public int Id { get; set; }

    public uint? IdModeloEquipamento { get; set; }

    public uint IdSala { get; set; }

    public string? Descricao { get; set; }

    public string TipoEquipamento { get; set; } = null!;

    public uint? IdHardwareDeSala { get; set; }

    /// <summary>
    /// L - LIGADO
    /// D - DESLIGADO
    /// </summary>
    public string Status { get; set; } = null!;

    public sbyte Temperatura { get; set; }

    public virtual Hardwaredesala? IdHardwareDeSalaNavigation { get; set; }

    public virtual Modeloequipamento? IdModeloEquipamentoNavigation { get; set; }

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual ICollection<Monitoramento> Monitoramentos { get; set; } = new List<Monitoramento>();
}
