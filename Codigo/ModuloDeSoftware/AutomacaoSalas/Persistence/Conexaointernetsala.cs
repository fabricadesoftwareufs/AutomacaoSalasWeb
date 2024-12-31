using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Conexaointernetsala
{
    public uint IdConexaoInternet { get; set; }

    public uint IdSala { get; set; }

    public int Prioridade { get; set; }

    public virtual Conexaointernet IdConexaoInternetNavigation { get; set; } = null!;

    public virtual Sala IdSalaNavigation { get; set; } = null!;
}
