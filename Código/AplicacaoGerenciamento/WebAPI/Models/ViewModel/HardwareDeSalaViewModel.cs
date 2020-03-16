namespace Model
{
    public class HardwareDeSalaViewModel
    {
        public int Id { get; set; }
        public string MAC { get; set; }
        public SalaModel SalaId { get; set; }
        public TipoHardwareModel TipoHardwareId { get; set; }
    }
}
