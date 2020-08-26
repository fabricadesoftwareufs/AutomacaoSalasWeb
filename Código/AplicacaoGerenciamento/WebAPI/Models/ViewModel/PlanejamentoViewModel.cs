using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class PlanejamentoViewModel
    {
        public const int DOMINGO = 0;
        public const int SEGUNDA_FEIRA = 1;
        public const int TERCA_FEIRA = 2;
        public const int QUARTA_FEIRA = 3;
        public const int QUINTA_FEIRA = 4;
        public const int SEXTA_FEIRA = 5;
        public const int SABADO = 6;

        public PlanejamentoModel Planejamento { get; set; }
        public SalaModel SalaId { get; set; }
        public UsuarioModel UsuarioId { get; set; }
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Período")]
        public string Periodo { get; set; }
        [Display(Name = "Horário")]
        public string Horario { get; set; }
        [Display(Name = "Dia da Semana")]
        public string DiaSemana { get; set; }
        [Display(Name = "Objetivo")]
        public string Objetivo { get; set; }

        public static int GetCodigoDia(string dia)
        {
            switch (dia)
            {
                case "DOM": return DOMINGO;
                case "SEG": return SEGUNDA_FEIRA;
                case "TER": return TERCA_FEIRA;
                case "QUA": return QUARTA_FEIRA;
                case "QUI": return QUINTA_FEIRA;
                case "SEX": return SEXTA_FEIRA;
                case "SAB": return SABADO;
                default: return -1;
            }
        }
    }
}
