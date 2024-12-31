using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Horariosala
{
    public uint Id { get; set; }

    public DateTime Data { get; set; }

    public TimeSpan HorarioInicio { get; set; }

    public TimeSpan HorarioFim { get; set; }

    public string Situacao { get; set; } = null!;

    public string Objetivo { get; set; } = null!;

    public uint IdUsuario { get; set; }

    public uint IdSala { get; set; }

    public uint? IdPlanejamento { get; set; }

    public virtual Planejamento? IdPlanejamentoNavigation { get; set; }

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
