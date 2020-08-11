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

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Organizacao")]
        public int OrganizacaoId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Título")]
        [StringLength(100, ErrorMessage = "Máximo são 100 caracteres")]
        public string Titulo { get; set; }
        public List<HardwareAuxModel> Hardwares { get; set; }

    }
}
