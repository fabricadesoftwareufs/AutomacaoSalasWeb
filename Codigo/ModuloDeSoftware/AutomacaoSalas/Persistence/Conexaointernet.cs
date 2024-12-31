using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Conexaointernet
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public uint IdBloco { get; set; }

    public virtual ICollection<Conexaointernetsala> Conexaointernetsalas { get; set; } = new List<Conexaointernetsala>();

    public virtual Bloco IdBlocoNavigation { get; set; } = null!;
}
