using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class SalaParticularModel
    {
        public SalaParticularModel()
        {
            Responsaveis = new List<UsuarioModel>();
        }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sala")]
        public int SalaId { get; set; }

        public List<UsuarioModel> Responsaveis { get; set; }
        public int BlocoSalas { get; set; }

    }
}
