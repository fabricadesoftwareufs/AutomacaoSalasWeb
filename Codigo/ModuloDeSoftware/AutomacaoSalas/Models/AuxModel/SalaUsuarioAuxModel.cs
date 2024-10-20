using Model.ViewModel;

namespace Model.AuxModel
{
    public class SalaUsuarioAuxModel
    {
        public SalaParticularModel SalaExclusiva { get; set; }
        public SalaModel Sala { get; set; }
        public MonitoramentoModel MonitoramentoLuzes { get; set; }
        public MonitoramentoModel MonitoramentoCondicionadores { get; set; }
        public BlocoModel Bloco { get; set; }
        public HorarioSalaModel HorarioSala { get; set; }
    }

}
