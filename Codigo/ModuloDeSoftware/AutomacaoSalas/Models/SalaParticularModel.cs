using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class SalaParticularModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public uint Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Usuário")]
        public uint UsuarioId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sala")]
        public uint SalaId { get; set; }
    }
}
