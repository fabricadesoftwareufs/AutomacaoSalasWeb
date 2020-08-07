using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class SalaParticularModel
    {


        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }
        [Required]
        [Display(Name = "Sala")]
        public int SalaId { get; set; }
    }
}
