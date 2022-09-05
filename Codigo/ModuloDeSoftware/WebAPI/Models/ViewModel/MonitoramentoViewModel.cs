
namespace Model.ViewModel
{
    public class MonitoramentoViewModel
    {
        public int Id { get; set; }
        public bool Estado { get; set; }
        public int SalaId { get; set; }
        public int EquipamentoId { get; set; }

        public bool SalaParticular { get; set; }
    }
}
