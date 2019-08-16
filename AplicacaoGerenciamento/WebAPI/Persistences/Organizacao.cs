using System.Collections.Generic;

namespace Persistences
{
    public partial class Organizacao
    {
        public Organizacao()
        {
            Bloco = new HashSet<Bloco>();
            UsuarioOrganizacoes = new HashSet<UsuarioOrganizacoes>();
        }

        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }

        public ICollection<Bloco> Bloco { get; set; }
        public ICollection<UsuarioOrganizacoes> UsuarioOrganizacoes { get; set; }
    }
}
