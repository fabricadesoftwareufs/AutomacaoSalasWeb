
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model.ViewModel
{
    public class ModeloEquipamentoViewModel
    {
        public uint Id { get; set; }

        [Display(Name = "Modelo do Equipamento")]
        public string Nome { get; set; }

        [Display(Name = "Marca do Equipamento")]
        public uint MarcaEquipamentoID { get; set; }

        [Display(Name = "Marca do Equipamento")]
        public string MarcaEquipamentoNome { get; set; }
        public List<MarcaEquipamentoViewModel> Marcas { get; set; } = new List<MarcaEquipamentoViewModel>();
    }
}
