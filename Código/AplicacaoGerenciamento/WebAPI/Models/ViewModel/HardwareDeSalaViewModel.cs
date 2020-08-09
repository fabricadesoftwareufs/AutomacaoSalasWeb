using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HardwareDeSalaViewModel
    {

        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "MAC")]
        public string MAC { get; set; }

        [Required]
        [Display(Name = "Sala")]
        public SalaModel SalaId { get; set; }

        [Required]
        [Display(Name = "Tipo de Hardware")]
        public TipoHardwareModel TipoHardwareId { get; set; }
    }
}
