using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Sala
    {
        public Sala()
        {
            Equipamento = new HashSet<Equipamento>();
            Hardwaredesala = new HashSet<Hardwaredesala>();
            Horariosala = new HashSet<Horariosala>();
            Planejamento  = new HashSet<Planejamento>();
            Salaparticular = new HashSet<Salaparticular>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Bloco { get; set; }

        public virtual Bloco BlocoNavigation { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
        public virtual ICollection<Hardwaredesala> Hardwaredesala { get; set; }
        public virtual ICollection<Horariosala> Horariosala { get; set; }
        public virtual ICollection<Planejamento> Planejamento  { get; set; }
        public virtual ICollection<Salaparticular> Salaparticular { get; set; }
    }
}
