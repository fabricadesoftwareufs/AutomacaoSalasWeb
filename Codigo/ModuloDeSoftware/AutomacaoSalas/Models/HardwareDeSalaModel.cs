using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class HardwareDeSalaModel
    {
        public enum TIPO : int
        {
            CONTROLADOR_SALA = 1,
            MODULO_ATUADOR = 2,
            MODULO_SENSOR = 3
        }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public uint Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "MAC")]
        [MinLength(17), MaxLength(17)]
        [StringLength(17, ErrorMessage = "O endereço MAC deve ter 12 caracteres")]
        public string MAC { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sala")]
        public uint SalaId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo de Hardware")]
        public uint TipoHardwareId { get; set; }
        [Display(Name = "Uuid")]
        [StringLength(75, ErrorMessage = "Máximo são 75 caracteres")]
        public string Uuid { get; set; }

        [StringLength(15, ErrorMessage = "Máximo são 15 caracteres")]
        public string Ip { get; set; }

        [StringLength(400, ErrorMessage = "Máximo são 400 caracteres")]
        public string Token { get; set; }
        public bool Registrado { get; set; }

        // Variaveis auxiliares
        public uint Bloco { get; set; }
        public uint Organizacao { get; set; }
        public int TipoEquipamento { get; set; }

    }
}
