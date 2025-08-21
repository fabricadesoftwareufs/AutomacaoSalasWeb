using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class AlterarSenhaViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha Atual")]
        [DataType(DataType.Password)]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nova Senha")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "A confirmação da senha não confere")]
        public string ConfirmarNovaSenha { get; set; }

        public uint UsuarioId { get; set; }
    }
}