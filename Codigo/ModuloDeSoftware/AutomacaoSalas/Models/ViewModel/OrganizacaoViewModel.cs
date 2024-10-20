using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class OrganizacaoViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Cnpj")]
        public string Cnpj { get; set; }
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }
    }
}
