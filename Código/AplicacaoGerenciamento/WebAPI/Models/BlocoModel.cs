using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class BlocoModel
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Organizacao")]
        public int OrganizacaoId { get; set; }
        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

    }
}
