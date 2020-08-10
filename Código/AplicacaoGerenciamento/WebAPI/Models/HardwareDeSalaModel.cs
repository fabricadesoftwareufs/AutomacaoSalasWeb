using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HardwareDeSalaModel
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "MAC")]
        public string MAC { get; set; }
        [Required]
        [Display(Name = "Bloco")]
        public int SalaId { get; set; }
        [Required]
        [Display(Name = "Tipo de Hardware")]
        public int TipoHardwareId { get; set; }
    }
}
