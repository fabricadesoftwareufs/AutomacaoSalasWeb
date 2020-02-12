using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Sala
    {
        public Sala()
        {
            Hardware = new HashSet<Hardware>();
            Horariosala = new HashSet<Horariosala>();
            Minhasala = new HashSet<Minhasala>();
            Planejamento = new HashSet<Planejamento>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Bloco { get; set; }

        public Bloco BlocoNavigation { get; set; }
        public ICollection<Hardware> Hardware { get; set; }
        public ICollection<Horariosala> Horariosala { get; set; }
        public ICollection<Minhasala> Minhasala { get; set; }
        public ICollection<Planejamento> Planejamento { get; set; }
    }
}
