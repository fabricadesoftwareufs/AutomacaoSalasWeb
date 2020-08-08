using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class OrganizacaoModel
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Cnpj")]
        public string Cnpj { get; set; }
        [Required]
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }
    }
}
