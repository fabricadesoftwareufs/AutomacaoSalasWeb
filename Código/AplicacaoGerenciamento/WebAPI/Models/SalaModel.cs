using Model.AuxModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class SalaModel
    {
        public SalaModel() { HardwaresSala = new List<HardwareAuxModel>();}

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Bloco")]
        [StringLength(100, ErrorMessage = "Máximo são 100 caracteres")]
        public int BlocoId { get; set; }
        public List<HardwareAuxModel> HardwaresSala { get; set; }
        public TipoHardwareModel TipoHardware { get; set; }

    }
}
