using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Bloco
    {
        public Bloco()
        {
            Salas = new HashSet<Sala>();
        }

        public uint Id { get; set; }
        public uint Organizacao { get; set; }
        public string Titulo { get; set; }

        public virtual Organizacao OrganizacaoNavigation { get; set; }
        public virtual ICollection<Sala> Salas { get; set; }
    }
}
