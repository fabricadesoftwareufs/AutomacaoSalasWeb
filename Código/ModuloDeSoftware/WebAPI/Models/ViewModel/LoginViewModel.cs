using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
