using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Organizacao
    {
        public Organizacao()
        {
            Blocos = new HashSet<Bloco>();
            Usuarioorganizacaos = new HashSet<Usuarioorganizacao>();
        }

        public uint Id { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }

        public virtual ICollection<Bloco> Blocos { get; set; }
        public virtual ICollection<Usuarioorganizacao> Usuarioorganizacaos { get; set; }
    }
}
