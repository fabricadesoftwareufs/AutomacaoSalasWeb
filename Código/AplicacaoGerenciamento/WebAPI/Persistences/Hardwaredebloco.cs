using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Hardwaredebloco
    {
        public int Id { get; set; }
        public string Mac { get; set; }
        public int Bloco { get; set; }
        public int TipoHardware { get; set; }

        public Bloco BlocoNavigation { get; set; }
        public Tipohardware TipoHardwareNavigation { get; set; }
    }
}
