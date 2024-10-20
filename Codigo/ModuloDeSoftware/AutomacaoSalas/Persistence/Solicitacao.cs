using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Solicitacao
{
    public uint Id { get; set; }

    public string Payload { get; set; } = null!;

    public uint IdHardware { get; set; }

    public uint IdHardwareAtuador { get; set; }

    public DateTime DataSolicitacao { get; set; }

    public DateTime? DataFinalizacao { get; set; }

    public string TipoSolicitacao { get; set; } = null!;

    public virtual Hardwaredesala IdHardwareAtuadorNavigation { get; set; } = null!;

    public virtual Hardwaredesala IdHardwareNavigation { get; set; } = null!;
}
