using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Modeloequipamento
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public uint IdMarcaEquipamento { get; set; }

    public virtual ICollection<Codigoinfravermelho> Codigoinfravermelhos { get; set; } = new List<Codigoinfravermelho>();

    public virtual ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();

    public virtual Marcaequipamento IdMarcaEquipamentoNavigation { get; set; } = null!;
}
