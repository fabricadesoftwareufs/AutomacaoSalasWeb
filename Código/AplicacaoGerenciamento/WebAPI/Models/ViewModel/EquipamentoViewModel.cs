using System.Collections.Generic;

namespace Model.ViewModel
{
    public class EquipamentoViewModel
    {
        public EquipamentoModel EquipamentoModel { set; get; }
        public List<CodigoInfravermelhoModel> Codigos { set; get; }
        public SalaModel SalaModel { set; get; } 
    }
}
