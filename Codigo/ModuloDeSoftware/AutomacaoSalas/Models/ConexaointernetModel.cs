using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ConexaointernetModel
    {
        public ConexaointernetModel()
        {
        }

        public uint Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "valor fora do intervalo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "valor fora do intervalo")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string ConfirmarSenha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha")]
        [MinLength(8), MaxLength(63)]
        [StringLength(16, ErrorMessage = "A senha deve ter entre 8 e 63 caracteres")]
        public uint IdBloco { get; set; }

        public string? NomeBloco { get; set; } 

    }
}
