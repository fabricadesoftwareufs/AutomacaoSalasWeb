using System.Collections.Generic;

namespace Model.ViewModel
{
    public class EquipamentoViewModel
    {
        public EquipamentoViewModel() { Codigos = new List<CodigoInfravermelhoViewModel>(); }
        public EquipamentoModel EquipamentoModel { set; get; }
        public List<CodigoInfravermelhoViewModel> Codigos { set; get; }
        public SalaModel SalaModel { set; get; }
        public BlocoModel BlocoModel { set; get; }
        public OrganizacaoModel OrganizacaoModel { set; get; }
    }
}
