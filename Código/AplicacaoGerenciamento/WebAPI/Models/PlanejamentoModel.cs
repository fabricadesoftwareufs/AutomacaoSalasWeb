using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class PlanejamentoModel
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInicio { get; set; }
        [Required]
        [Display(Name = "Data de Término")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFim { get; set; }
        [Required]
        [Display(Name = "Horário de Início")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "HH:mm", ApplyFormatInEditMode = true)]
        public TimeSpan HorarioInicio { get; set; }
        [Required]
        [Display(Name = "Horário de Término")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "HH:mm", ApplyFormatInEditMode = true)]
        public TimeSpan HorarioFim { get; set; }
        [Required]
        [Display(Name = "Dia da Semana")]
        public string DiaSemana { get; set; }
        [Required]
        [Display(Name = "Objetivo")]
        public string Objetivo { get; set; }
        [Required]
        [Display(Name = "Sala")]
        public int SalaId { get; set; }
        [Required]
        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }
    }
}
