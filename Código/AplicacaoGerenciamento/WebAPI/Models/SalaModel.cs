using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class SalaModel
    {

        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }
        [Required]
        [Display(Name = "Bloco")]
        public int BlocoId { get; set; }
    }
}
