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
        public int Bloco { get; set; }
        public int Organizacao { get; set; }
    }
}
