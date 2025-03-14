using System.Collections.Generic;

namespace Model.AuxModel
{
    public class SalaAuxModel
    {
        public SalaAuxModel() { 
            HardwaresSala = new List<HardwareAuxModel>();
            ConexaoInternetSala = new List<ConexaoInternetSalaModel>();
            EquipamentoSala = new List<EquipamentoModel>();
            HardwaresSala2 = new List<HardwareDeSalaViewModel>();
        }
        public SalaModel Sala { get; set; }
        public uint OrganizacaoId { get; set; }
        public List<HardwareAuxModel> HardwaresSala { get; set; }

        public List<ConexaoInternetSalaModel> ConexaoInternetSala { get; set; }

        public List<EquipamentoModel> EquipamentoSala { get; set; }

        public List<HardwareDeSalaViewModel> HardwaresSala2 { get; set; }

    }
}
