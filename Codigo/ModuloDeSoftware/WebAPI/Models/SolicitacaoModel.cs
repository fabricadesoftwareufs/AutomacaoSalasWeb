using System;
using System.Collections.Generic;

namespace Model
{
    public partial class SolicitacaoModel
    {
        public int Id { get; set; }
        public string Payload { get; set; }
        public int IdHardware { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataFinalizacao { get; set; }

        public virtual HardwareDeSalaModel HardwareNavigation { get; set; }
    }
}
