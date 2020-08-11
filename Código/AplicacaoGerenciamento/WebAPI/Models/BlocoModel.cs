using Model.AuxModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class BlocoModel
    {

        public BlocoModel()
        {
            Hardwares = new List<HardwareAuxModel>();
        }

        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Organizacao")]
        public int OrganizacaoId { get; set; }
        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        public List<HardwareAuxModel> Hardwares { get; set; }

    }
}
