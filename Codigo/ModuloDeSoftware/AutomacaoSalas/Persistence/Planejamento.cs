using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Planejamento
{
    public uint Id { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public TimeSpan HorarioInicio { get; set; }

    public TimeSpan HorarioFim { get; set; }

    public string DiaSemana { get; set; } = null!;

    public string Objetivo { get; set; } = null!;

    public uint Usuario { get; set; }

    public uint Sala { get; set; }

    public virtual ICollection<Horariosala> Horariosalas { get; set; } = new List<Horariosala>();

    public virtual Sala SalaNavigation { get; set; } = null!;

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
