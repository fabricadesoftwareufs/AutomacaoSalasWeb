using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HorarioSalaModel
    {
        public static string SITUACAO_APROVADA = "APROVADA";
        public static string SITUACAO_CANCELADA = "CANCELADA";
        public static string SITUACAO_REPROVADA = "REPROVADA";
        public static string SITUACAO_PENDENTE = "PENDENTE";

        public HorarioSalaModel()
        {
            Planejamento = null;
        }

        [Required(ErrorMessage = "Campo obrigatório")]
        [JsonProperty(PropertyName = "id")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [JsonProperty(PropertyName = "data")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Horário de ínicio")]
        [DataType(DataType.Time)]
        [JsonProperty(PropertyName = "horario-inicial")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HorarioInicio { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Horário de Termino")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "horario-final")]
        public TimeSpan HorarioFim { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "status")]
        [JsonProperty(PropertyName = "status")]
        [StringLength(45, ErrorMessage = "Máximo são 45 caracteres")]
        public string Situacao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Objetivo")]
        [JsonProperty(PropertyName = "objective")]
        [StringLength(500, ErrorMessage = "Máximo são 500 caracteres")]
        public string Objetivo { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [JsonProperty(PropertyName = "id-usuario")]
        [Display(Name = "Responsável")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [JsonProperty(PropertyName = "id-sala")]
        [Display(Name = "Sala")]
        public int SalaId { get; set; }
        public int? Planejamento { get; set; }
    }
}
