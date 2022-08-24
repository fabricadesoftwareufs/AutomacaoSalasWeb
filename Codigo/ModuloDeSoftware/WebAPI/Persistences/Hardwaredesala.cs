using System;
using System.Collections.Generic;



namespace Persistence
{
    public partial class Hardwaredesala
    {
        public Hardwaredesala()
        {
            Equipamento = new HashSet<Equipamento>();
            Solicitacao = new HashSet<Solicitacao>();
        }

        public int Id { get; set; }
        public string Mac { get; set; }
        public int Sala { get; set; }
        public int TipoHardware { get; set; }
        public string Ip { get; set; }
        public string Uuid { get; set; }
        public string Token { get; set; }
        public byte Registrado { get; set; }

        public virtual Sala SalaNavigation { get; set; }
        public virtual Tipohardware TipoHardwareNavigation { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
        public virtual ICollection<Solicitacao> Solicitacao { get; set; }
    }
}
