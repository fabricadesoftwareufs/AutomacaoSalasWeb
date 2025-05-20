using System;
namespace Model.ViewModel
{
    public class MonitoramentoViewModel
    {
        public int Id { get; set; }
        public int SalaId { get; set; }
        public int EquipamentoId { get; set; }

        public int OperacaoId { get; set; }
        public DateTime DataHora { get; set; } = DateTime.Now;
        public uint UsuarioId { get; set; }
        public int Temperatura { get; set; } = 0;


        public bool SalaParticular { get; set; }
    }
}
