using System.Collections.Generic;

namespace Model.AuxModel
{
    public class SalaAuxModel
    {
        public SalaAuxModel() { HardwaresSala = new List<HardwareAuxModel>(); }
        public SalaModel Sala { get; set; }
        public int OrganizacaoId { get; set; }
        public List<HardwareAuxModel> HardwaresSala { get; set; }
    }
}
