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

    public uint Usuario { get; set; }

    public uint Sala { get; set; }

    public uint? Planejamento { get; set; }

    public virtual Planejamento? PlanejamentoNavigation { get; set; }

    public virtual Sala SalaNavigation { get; set; } = null!;

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
