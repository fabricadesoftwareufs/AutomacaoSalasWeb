using Model.ViewModel;
using System.Collections.Generic;

namespace Model.AuxModel
{
    public class SalaUsuarioAuxModel
    {
        public SalaParticularModel SalaExclusiva { get; set; }
        public SalaModel Sala { get; set; }
        public List<MonitoramentoViewModel> MonitoramentoLuzes { get; set; }
        public List<MonitoramentoViewModel> MonitoramentoCondicionadores { get; set; }
        public BlocoModel Bloco { get; set; }
        public HorarioSalaModel HorarioSala { get; set; }
    }

}
