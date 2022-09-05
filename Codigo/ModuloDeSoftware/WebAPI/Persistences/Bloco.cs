using System;
using System.Collections.Generic;



namespace Persistence
{
    public partial class Bloco
    {
        public Bloco()
        {
            Sala = new HashSet<Sala>();
        }

        public int Id { get; set; }
        public int Organizacao { get; set; }
        public string Titulo { get; set; }

        public virtual Organizacao OrganizacaoNavigation { get; set; }
        public virtual ICollection<Sala> Sala { get; set; }
    }
}
