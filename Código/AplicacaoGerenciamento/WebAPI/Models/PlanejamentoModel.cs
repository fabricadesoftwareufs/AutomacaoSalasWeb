using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class PlanejamentoModel
    {

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataInicio { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data de Término")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataFim { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]

        [Display(Name = "Horário de Início")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HorarioInicio { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]

        [Display(Name = "Horário de Término")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HorarioFim { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]

        [Display(Name = "Dia da Semana")]
        public string DiaSemana { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Objetivo")]
        [StringLength(500, ErrorMessage = "Máximo são 500 caracteres")]

        public string Objetivo { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]

        [Display(Name = "Sala")]
        public int SalaId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]

        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }
    

    }
}
