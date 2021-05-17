namespace Model
{
    public class HardwareDeBlocoViewModel
    {
        public int Id { get; set; }
        public string MAC { get; set; }
        public BlocoModel BlocoId { get; set; }
        public TipoHardwareModel TipoHardwareId { get; set; }
    }
}
