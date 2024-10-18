using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Equipamento
{
    public int Id { get; set; }

    public string Modelo { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string? Descricao { get; set; }

    public uint Sala { get; set; }

    public string TipoEquipamento { get; set; } = null!;

    public uint? HardwareDeSala { get; set; }

    public virtual ICollection<Codigoinfravermelho> Codigoinfravermelhos { get; set; } = new List<Codigoinfravermelho>();

    public virtual Hardwaredesala? HardwareDeSalaNavigation { get; set; }

    public virtual Sala SalaNavigation { get; set; } = null!;
}
