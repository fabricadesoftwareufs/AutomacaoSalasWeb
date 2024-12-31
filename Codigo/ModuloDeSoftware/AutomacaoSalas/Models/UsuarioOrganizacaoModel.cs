using System;

namespace Model
{
    public class UsuarioOrganizacaoModel
    {
        public uint UsuarioId { get; set; }
        public uint OrganizacaoId { get; set; }
        public uint IdTipoUsuario { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
