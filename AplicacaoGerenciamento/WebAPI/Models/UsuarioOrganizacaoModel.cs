using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UsuarioOrganizacaoModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int OrganizacaoId { get; set; }
    }
}
