using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HardwareDeBlocoModel
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "MAC")]
        [MaxLength(10), MinLength(10)]
        public string MAC { get; set; }
        [Required]
        [Display(Name = "Bloco")]
        public int BlocoId { get; set; }
        [Required]
        [Display(Name = "Tipo de Hardware")]
        public int TipoHardwareId { get; set; }
    }
}
