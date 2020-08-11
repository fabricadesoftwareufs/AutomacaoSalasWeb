using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HorarioSalaModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Horário de ínicio")]
        public TimeSpan HorarioInicio { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Horário de Termino")]
        public TimeSpan HorarioFim { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Situação")]
        [StringLength(45, ErrorMessage = "Máximo são 45 caracteres")]
        public string Situacao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Objetivo")]
        [StringLength(500, ErrorMessage = "Máximo são 500 caracteres")]
        public string Objetivo { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Responsável")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sala")]
        public int SalaId { get; set; }

    }
}
