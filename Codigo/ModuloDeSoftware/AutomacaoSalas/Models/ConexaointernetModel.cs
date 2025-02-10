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
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome da Rede")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "O nome deve ter entre 4 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(63, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 63 caracteres")]
        public string Senha { get; set; }

        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string ConfirmarSenha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public uint IdBloco { get; set; }

        [Display(Name = "Nome do Bloco")]
        public string? NomeBloco { get; set; }
        
        public uint? OrganizacaoId { get; set; }

    }
}
