using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Solicitacao
    {
        public uint Id { get; set; }
        public uint IdHardware { get; set; }
        public string Payload { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public string TipoSolicitacao { get; set; }

        public virtual Hardwaredesala IdHardwareNavigation { get; set; }
    }
}
