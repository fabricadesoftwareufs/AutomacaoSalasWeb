using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Codigoinfravermelho
{
    public int Id { get; set; }

    public int Equipamento { get; set; }

    public int Operacao { get; set; }

    public string Codigo { get; set; } = null!;

    public virtual Equipamento EquipamentoNavigation { get; set; } = null!;

    public virtual Operacao OperacaoNavigation { get; set; } = null!;
}
