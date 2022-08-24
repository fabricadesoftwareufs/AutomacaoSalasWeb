using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Tipohardware
    {
        public Tipohardware()
        {
            Hardwaredesalas = new HashSet<Hardwaredesala>();
        }

        public uint Id { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<Hardwaredesala> Hardwaredesalas { get; set; }
    }
}
