using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Monitoramento
{
    public int Id { get; set; }

    public int IdEquipamento { get; set; }

    public int IdOperacao { get; set; }

    public DateTime DataHora { get; set; }

    public uint IdUsuario { get; set; }

    public int Temperatura { get; set; }

    public virtual Equipamento IdEquipamentoNavigation { get; set; } = null!;

    public virtual Operacao IdOperacaoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
