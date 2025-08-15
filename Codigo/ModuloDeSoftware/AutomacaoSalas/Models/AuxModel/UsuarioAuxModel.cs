using System.Collections.Generic;

namespace Model.AuxModel
{
    public class UsuarioAuxModel
    {
        public UsuarioModel UsuarioModel { get; set; }
        public TipoUsuarioModel TipoUsuarioModel { get; set; }
        public List<OrganizacaoModel> OrganizacaoModels { get; set; }
        public string Email { get; set; }
    }
}
