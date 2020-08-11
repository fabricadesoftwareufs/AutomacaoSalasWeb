using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cpf")]
        [StringLength(11, ErrorMessage = "Máximo são 11 caracteres")] 
        public string Cpf { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(45, ErrorMessage = "Máximo são 45 caracteres")]
        public string Nome { get; set; }
        [Display(Name = "Data de Nascimento")]
        [StringLength(500, ErrorMessage = "Máximo são 500 caracteres")]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha")]
        [StringLength(100, ErrorMessage = "Máximo são 100 caracteres")] 
        public string Senha { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo")]
        [StringLength(500, ErrorMessage = "Máximo são 500 caracteres")]

        public int TipoUsuarioId { get; set; }
    }
}
