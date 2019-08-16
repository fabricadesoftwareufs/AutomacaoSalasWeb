using System;
using System.Collections.Generic;

namespace Persistences
{
    public partial class Disciplina
    {
        public Disciplina()
        {
            Horariosala = new HashSet<Horariosala>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }

        public ICollection<Horariosala> Horariosala { get; set; }
    }
}
