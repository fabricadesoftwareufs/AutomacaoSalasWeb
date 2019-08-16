using System;
using System.Collections.Generic;

namespace Persistences
{
    public partial class Sala
    {
        public Sala()
        {
            Hardware = new HashSet<Hardware>();
            Horariosala = new HashSet<Horariosala>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Bloco { get; set; }

        public Bloco BlocoNavigation { get; set; }
        public ICollection<Hardware> Hardware { get; set; }
        public ICollection<Horariosala> Horariosala { get; set; }
    }
}
