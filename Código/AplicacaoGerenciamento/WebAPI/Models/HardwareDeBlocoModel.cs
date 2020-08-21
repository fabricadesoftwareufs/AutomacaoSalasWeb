using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HardwareDeBlocoModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "MAC")]
        [MinLength(17), MaxLength(17)]
        [StringLength(17, ErrorMessage = "O endereço MAC deve ter 12 caracteres")]

        public string MAC { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Bloco")]
        public int BlocoId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo de Hardware")]
        public int TipoHardwareId { get; set; }

        // auxiliar
        public int Organizacao { get; set; }
    }
}
