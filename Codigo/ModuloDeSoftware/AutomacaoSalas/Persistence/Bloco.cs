using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Bloco
{
    public uint Id { get; set; }

    public uint IdOrganizacao { get; set; }

    public string Titulo { get; set; } = null!;

    public virtual ICollection<Conexaointernet> Conexaointernets { get; set; } = new List<Conexaointernet>();

    public virtual Organizacao IdOrganizacaoNavigation { get; set; } = null!;

    public virtual ICollection<Sala> Salas { get; set; } = new List<Sala>();
}
