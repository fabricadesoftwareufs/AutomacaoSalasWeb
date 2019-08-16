using System;
using System.Collections.Generic;

namespace Persistences
{
    public partial class Usuario
    {
        public Usuario()
        {
            Horariosala = new HashSet<Horariosala>();
            UsuarioOrganizacoes = new HashSet<UsuarioOrganizacoes>();
        }

        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Senha { get; set; }
        public int TipoUsuario { get; set; }

        public Tipousuario TipoUsuarioNavigation { get; set; }
        public ICollection<Horariosala> Horariosala { get; set; }
        public ICollection<UsuarioOrganizacoes> UsuarioOrganizacoes { get; set; }
    }
}
