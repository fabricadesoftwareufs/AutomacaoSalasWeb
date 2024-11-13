using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class OrganizacaoModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public uint Id { get; set; }
       
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "CNPJ")]
        [StringLength(18, ErrorMessage = "Máximo são 14 caracteres")]
        public string Cnpj { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Razão Social")]
        [StringLength(45, ErrorMessage = "Máximo são 45 caracteres")]
        public string RazaoSocial { get; set; }
    }
}
