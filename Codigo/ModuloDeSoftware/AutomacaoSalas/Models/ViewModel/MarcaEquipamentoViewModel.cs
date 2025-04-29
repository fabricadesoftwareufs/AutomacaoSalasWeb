
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model.ViewModel
{
    public class MarcaEquipamentoViewModel
    {
        public MarcaEquipamentoViewModel()
        {
        }

        [Display(Name = "Código")]
        public uint Id { get; set; }

        [Display(Name = "Marca do Equipamento")]
        public string Nome { get; set; }
    }
}
