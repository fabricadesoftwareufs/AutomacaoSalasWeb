using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Hardwaredesala
{
    public uint Id { get; set; }

    public string Mac { get; set; } = null!;

    public uint IdSala { get; set; }

    public uint IdTipoHardware { get; set; }

    public string? Ip { get; set; }

    public string? Uuid { get; set; }

    public string Token { get; set; } = null!;

    public sbyte Registrado { get; set; }

    public virtual ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual Tipohardware IdTipoHardwareNavigation { get; set; } = null!;

    public virtual ICollection<Solicitacao> SolicitacaoIdHardwareAtuadorNavigations { get; set; } = new List<Solicitacao>();

    public virtual ICollection<Solicitacao> SolicitacaoIdHardwareNavigations { get; set; } = new List<Solicitacao>();
}
