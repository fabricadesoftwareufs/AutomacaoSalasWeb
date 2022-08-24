using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Sala
    {
        public Sala()
        {
            Equipamentos = new HashSet<Equipamento>();
            Hardwaredesalas = new HashSet<Hardwaredesala>();
            Horariosalas = new HashSet<Horariosala>();
            Planejamentos = new HashSet<Planejamento>();
            Salaparticulars = new HashSet<Salaparticular>();
        }

        public uint Id { get; set; }
        public string Titulo { get; set; }
        public uint Bloco { get; set; }

        public virtual Bloco BlocoNavigation { get; set; }
        public virtual ICollection<Equipamento> Equipamentos { get; set; }
        public virtual ICollection<Hardwaredesala> Hardwaredesalas { get; set; }
        public virtual ICollection<Horariosala> Horariosalas { get; set; }
        public virtual ICollection<Planejamento> Planejamentos { get; set; }
        public virtual ICollection<Salaparticular> Salaparticulars { get; set; }
    }
}
