using System.Collections.Generic;

namespace Persistence
{
    public partial class Tipohardware
    {
        public Tipohardware()
        {
            Hardwaredebloco = new HashSet<Hardwaredebloco>();
            Hardwaredesala = new HashSet<Hardwaredesala>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }

        public ICollection<Hardwaredebloco> Hardwaredebloco { get; set; }
        public ICollection<Hardwaredesala> Hardwaredesala { get; set; }
    }
}
