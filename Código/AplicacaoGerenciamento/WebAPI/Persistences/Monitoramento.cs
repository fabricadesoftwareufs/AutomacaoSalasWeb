using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Monitoramento
    {
        public int Id { get; set; }
        public byte Luzes { get; set; }
        public byte ArCondicionado { get; set; }
        public int Sala { get; set; }

        public Sala SalaNavigation { get; set; }
    }
}
