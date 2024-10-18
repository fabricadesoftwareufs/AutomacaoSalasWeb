using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Bloco
{
    public uint Id { get; set; }

    public uint Organizacao { get; set; }

    public string Titulo { get; set; } = null!;

    public virtual Organizacao OrganizacaoNavigation { get; set; } = null!;

    public virtual ICollection<Sala> Salas { get; set; } = new List<Sala>();
}
