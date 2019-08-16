using System.Collections.Generic;

namespace Persistences
{
    public partial class Tipohardware
    {
        public Tipohardware()
        {
            Hardware = new HashSet<Hardware>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }

        public ICollection<Hardware> Hardware { get; set; }
    }
}
