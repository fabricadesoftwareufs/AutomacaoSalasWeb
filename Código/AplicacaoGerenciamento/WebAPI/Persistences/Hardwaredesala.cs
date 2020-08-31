using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Hardwaredesala
    {
        public int Id { get; set; }
        public string Mac { get; set; }
        public int Sala { get; set; }
        public int TipoHardware { get; set; }

        public Sala SalaNavigation { get; set; }
        public Tipohardware TipoHardwareNavigation { get; set; }
    }
}
