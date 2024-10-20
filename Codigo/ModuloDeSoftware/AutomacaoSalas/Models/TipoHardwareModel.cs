namespace Model
{
    public class TipoHardwareModel
    {
        public const int CONTROLADOR_DE_SALA = 1;
        public const int CONTROLADOR_DE_DISPOSITIVO = 2;
        public const int MODEULO_DE_SENSORIAMENTO = 3;
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}
