using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Codigoinfravermelho
{
    public int Id { get; set; }

    public int IdEquipamento { get; set; }

    public int IdOperacao { get; set; }

    public string Codigo { get; set; } = null!;

    public virtual Equipamento IdEquipamentoNavigation { get; set; } = null!;

    public virtual Operacao IdOperacaoNavigation { get; set; } = null!;
}
