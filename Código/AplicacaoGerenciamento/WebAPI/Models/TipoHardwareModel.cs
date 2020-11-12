namespace Model
{
    public class TipoHardwareModel
    {
        public const int CONTROLADOR_DE_BLOCO = 1;
        public const int CONTROLADOR_DE_SALA = 2;
        public const int MODEULO_DE_SENSORIAMENTO = 3;
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}
