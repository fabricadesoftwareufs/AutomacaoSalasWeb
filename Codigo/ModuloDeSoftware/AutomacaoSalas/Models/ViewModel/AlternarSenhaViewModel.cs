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
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres")]
        [MaxLength(16, ErrorMessage = "A senha deve ter no máximo 16 caracteres")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "A confirmação da senha não confere")]
        public string ConfirmarNovaSenha { get; set; }

        public uint UsuarioId { get; set; }
    }
}
