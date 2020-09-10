using System.Collections.Generic;

namespace Model.AuxModel
{
    public class SalaParticularAuxModel
    {
        public SalaParticularAuxModel()
        {
            Responsaveis = new List<ResponsavelAuxModel>();
        }

        public SalaParticularModel SalaParticular { get; set; }
        public List<ResponsavelAuxModel> Responsaveis { get; set; }
        public int BlocoSalas { get; set; }
        public int Organizacao { get; set; }
    }
}
