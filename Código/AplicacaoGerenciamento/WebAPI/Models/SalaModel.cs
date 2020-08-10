using Model.AuxModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class SalaModel
    {
        public SalaModel() { HardwaresSala = new List<HardwareAuxModel>();}

        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }
        [Required]
        [Display(Name = "Bloco")]
        public int BlocoId { get; set; }
        public List<HardwareAuxModel> HardwaresSala { get; set; }
        public TipoHardwareModel TipoHardware { get; set; }

    }
}
