using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(11, ErrorMessage = "CPF só possui 11 caracteres")]
        public string Login { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
