using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Usuario
    {
        public Usuario()
        {
            Horariosala = new HashSet<Horariosala>();
            Planejamento = new HashSet<Planejamento>();
            Salaparticular = new HashSet<Salaparticular>();
            Usuarioorganizacao = new HashSet<Usuarioorganizacao>();
        }

        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Senha { get; set; }
        public int TipoUsuario { get; set; }

        public Tipousuario TipoUsuarioNavigation { get; set; }
        public ICollection<Horariosala> Horariosala { get; set; }
        public ICollection<Planejamento> Planejamento { get; set; }
        public ICollection<Salaparticular> Salaparticular { get; set; }
        public ICollection<Usuarioorganizacao> Usuarioorganizacao { get; set; }
    }
}
