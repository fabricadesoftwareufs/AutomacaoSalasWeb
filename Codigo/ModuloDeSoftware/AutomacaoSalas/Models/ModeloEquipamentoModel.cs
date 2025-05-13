using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ModeloEquipamentoModel
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Modelo do Equipamento")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Marca do Equipamento")]
        public uint MarcaEquipamentoID { get; set; }


        public MarcaEquipamentoModel? Marca { get; set; }
    }
}
