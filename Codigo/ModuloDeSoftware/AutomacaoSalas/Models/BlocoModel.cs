using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class BlocoModel
    {

        public BlocoModel()
        {
        }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public uint Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Organizacao")]
        public uint OrganizacaoId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Título")]
        [StringLength(100, ErrorMessage = "Máximo são 100 caracteres")]
        public string Titulo { get; set; }
    }
}
