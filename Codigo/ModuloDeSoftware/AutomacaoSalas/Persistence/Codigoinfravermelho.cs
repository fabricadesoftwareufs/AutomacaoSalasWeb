using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Codigoinfravermelho
{
    public int Id { get; set; }

    public int IdOperacao { get; set; }

    public uint IdModeloEquipamento { get; set; }

    public string Codigo { get; set; } = null!;

    public virtual Modeloequipamento IdModeloEquipamentoNavigation { get; set; } = null!;

    public virtual Operacao IdOperacaoNavigation { get; set; } = null!;
}
