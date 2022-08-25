using System;
using System.Collections.Generic;



namespace Persistence
{
    public partial class Solicitacao
    {
        public int Id { get; set; }
        public int IdHardware { get; set; }
        public string Payload { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public string TipoSolicitacao { get; set; }

        public virtual Hardwaredesala IdHardwareNavigation { get; set; }
    }
}
