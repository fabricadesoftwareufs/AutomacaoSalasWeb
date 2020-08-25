using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.AuxModel
{
    public class PlanejamentoAuxModel
    {
        public PlanejamentoAuxModel() { Horarios = new List<HorarioPlanejamentoAuxModel>(); }
        public PlanejamentoModel Planejamento { get; set; }
        public List<HorarioPlanejamentoAuxModel> Horarios { get; set; }
        public int Bloco { get; set; }
        public int Organizacao { get; set; }

    }
}
