﻿using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class SalaModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public uint Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Bloco")]
        public uint BlocoId { get; set; }
    }
}
