using System;

namespace Model
{
    public partial class SolicitacaoModel
    {
        public static string ATUALIZAR_RESERVAS = "ATUALIZAR_RESERVAS";
        public static string MONITORAMENTO_AR_CONDICIONADO = "MONITORAMENTO_AR_CONDICIONADO";
        public static string MONITORAMENTO_LUZES = "MONITORAMENTO_LUZES";

        public uint Id { get; set; }
        public string Payload { get; set; }
        public uint IdHardware { get; set; }
        public uint IdHardwareAtuador { get; set; }

        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public string TipoSolicitacao { get; set; }

        public virtual HardwareDeSalaModel HardwareNavigation { get; set; }
    }
}
