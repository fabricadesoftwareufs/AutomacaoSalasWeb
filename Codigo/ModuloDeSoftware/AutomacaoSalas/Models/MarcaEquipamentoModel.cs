using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class MarcaEquipamentoModel
    {
        public MarcaEquipamentoModel()
        {
        }
        public uint Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Marca do Equipamento")]
        public string Nome { get; set; }

    }
}
