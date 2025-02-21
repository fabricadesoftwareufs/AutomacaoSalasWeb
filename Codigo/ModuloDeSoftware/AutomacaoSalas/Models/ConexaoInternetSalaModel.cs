using System;


namespace Model
{
    public class ConexaoInternetSalaModel
    {
        public uint ConexaoInternetId { get; set; }
        public uint SalaId { get; set; }
        public int Prioridade { get; set; }

        public string? Ssid { get; set; }
    }
}
