using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Model.ViewModel
{
    public class UsuarioViewModel
    {
        public UsuarioModel UsuarioModel { get; set; }
        public TipoUsuarioModel TipoUsuarioModel { get; set; }
        
        [Display(Name = "Organização")]
        public OrganizacaoModel OrganizacaoModel { get; set; }

        public UsuarioOrganizacaoModel UsuarioOrganizacaoModel { get; set; }

    }
}
