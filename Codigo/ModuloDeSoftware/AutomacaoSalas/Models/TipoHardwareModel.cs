using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class TipoHardwareModel
    {
        public const int CONTROLADOR_DE_SALA = 1;
        public const int CONTROLADOR_DE_DISPOSITIVO = 2;
        public const int MODEULO_DE_SENSORIAMENTO = 3;
        public uint Id { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(45, ErrorMessage = "Máximo são 45 caracteres")]
        public string Descricao { get; set; }
    }
}
