using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Sala
    {
        public Sala()
        {
            Hardwaredesala = new HashSet<Hardwaredesala>();
            Horariosala = new HashSet<Horariosala>();
            Monitoramento = new HashSet<Monitoramento>();
            Planejamento = new HashSet<Planejamento>();
            Salaparticular = new HashSet<Salaparticular>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Bloco { get; set; }

        public Bloco BlocoNavigation { get; set; }
        public ICollection<Hardwaredesala> Hardwaredesala { get; set; }
        public ICollection<Horariosala> Horariosala { get; set; }
        public ICollection<Monitoramento> Monitoramento { get; set; }
        public ICollection<Planejamento> Planejamento { get; set; }
        public ICollection<Salaparticular> Salaparticular { get; set; }
    }
}
