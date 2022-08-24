using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Hardwaredesala
    {
        public Hardwaredesala()
        {
            Equipamentos = new HashSet<Equipamento>();
            Solicitacaos = new HashSet<Solicitacao>();
        }

        public uint Id { get; set; }
        public string Mac { get; set; }
        public uint Sala { get; set; }
        public uint TipoHardware { get; set; }
        public string Ip { get; set; }
        public string Uuid { get; set; }
        public string Token { get; set; }
        public sbyte Registrado { get; set; }

        public virtual Sala SalaNavigation { get; set; }
        public virtual Tipohardware TipoHardwareNavigation { get; set; }
        public virtual ICollection<Equipamento> Equipamentos { get; set; }
        public virtual ICollection<Solicitacao> Solicitacaos { get; set; }
    }
}
