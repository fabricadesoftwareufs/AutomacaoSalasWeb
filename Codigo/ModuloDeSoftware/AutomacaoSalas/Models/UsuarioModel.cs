using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public uint Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cpf")]
        [StringLength(14, ErrorMessage = "Máximo são 11 caracteres")]
        public string Cpf { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(45, ErrorMessage = "Máximo são 45 caracteres")]
        public string Nome { get; set; }
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha")]
        /* [MinLength(8), MaxLength(16)]
        [StringLength(16, ErrorMessage = "A senha deve ter entre 8 e 16 caracteres")] */
        public string Senha { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo")]

        public uint TipoUsuarioId { get; set; }
    }
}
