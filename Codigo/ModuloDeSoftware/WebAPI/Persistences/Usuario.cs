using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Usuario
    {
        public Usuario()
        {
            Horariosalas = new HashSet<Horariosala>();
            Planejamentos = new HashSet<Planejamento>();
            Salaparticulars = new HashSet<Salaparticular>();
            Usuarioorganizacaos = new HashSet<Usuarioorganizacao>();
        }

        public uint Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Senha { get; set; }
        public uint TipoUsuario { get; set; }

        public virtual Tipousuario TipoUsuarioNavigation { get; set; }
        public virtual ICollection<Horariosala> Horariosalas { get; set; }
        public virtual ICollection<Planejamento> Planejamentos { get; set; }
        public virtual ICollection<Salaparticular> Salaparticulars { get; set; }
        public virtual ICollection<Usuarioorganizacao> Usuarioorganizacaos { get; set; }
    }
}
