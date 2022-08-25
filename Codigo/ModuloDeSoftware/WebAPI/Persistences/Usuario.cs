using System;
using System.Collections.Generic;



namespace Persistence
{
    public partial class Usuario
    {
        public Usuario()
        {
            Horariosala = new HashSet<Horariosala>();
            Planejamento  = new HashSet<Planejamento>();
            Salaparticular = new HashSet<Salaparticular>();
            Usuarioorganizacao = new HashSet<Usuarioorganizacao>();
        }

        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Senha { get; set; }
        public int TipoUsuario { get; set; }

        public virtual Tipousuario TipoUsuarioNavigation { get; set; }
        public virtual ICollection<Horariosala> Horariosala { get; set; }
        public virtual ICollection<Planejamento> Planejamento  { get; set; }
        public virtual ICollection<Salaparticular> Salaparticular { get; set; }
        public virtual ICollection<Usuarioorganizacao> Usuarioorganizacao { get; set; }
    }
}
