using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Tipohardware
{
    public uint Id { get; set; }

    public string Descricao { get; set; } = null!;

    public uint IdOrganizacao { get; set; }

    public virtual ICollection<Hardwaredesala> Hardwaredesalas { get; set; } = new List<Hardwaredesala>();

    public virtual Organizacao IdOrganizacaoNavigation { get; set; } = null!;
}
