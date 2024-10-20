using Model.ViewModel;
using System.Collections.Generic;

namespace Model.AuxModel
{
    public class PlanejamentoAuxModel
    {
        public PlanejamentoAuxModel() { Horarios = new List<HorarioPlanejamentoAuxModel>(); }

        public PlanejamentoModel Planejamento { get; set; }
        public List<HorarioPlanejamentoAuxModel> Horarios { get; set; }

        /* atributos auxiliares */
        public uint Bloco { get; set; }
        public uint Organizacao { get; set; }
    }
}
