using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HardwareDeSalaModel
    {
        public enum TIPO : int
        {
            CONTROLADOR_SALA = 1,
            MODULO_SENSOR = 2,
            MODULO_DISPOSITIVO = 3
        }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "MAC")]
        [MinLength(17), MaxLength(17)]
        [StringLength(17, ErrorMessage = "O endereço MAC deve ter 12 caracteres")]
        public string MAC { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sala")]
        public int SalaId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo de Hardware")]
        public int TipoHardwareId { get; set; }
        [Display(Name = "Uuid")]
        public string Uuid { get; set; }

        public string Ip { get; set; }
        public string Token { get; set; }
        // Variaveis auxiliares
        public int Bloco { get; set; }
        public int Organizacao { get; set; }
    }
}
