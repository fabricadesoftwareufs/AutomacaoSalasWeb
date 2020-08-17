using System;
using System.Collections.Generic;
using System.Text;

namespace Model.AuxModel
{
    public class SalaUsuarioAuxModel
    {
        public SalaParticularModel SalaExclusiva { get; set; }
        public SalaModel Sala { get; set; }
        public MonitoramentoModel Monitoramento { get; set; }
        public BlocoModel Bloco { get; set; }
        public HorarioSalaModel HorarioSala { get; set; }
    }

}
