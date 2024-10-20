using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Model
{
    public class TipoUsuarioModel
    {
        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_GESTOR = "GESTOR";
        public const string ROLE_COLABORADOR = "COLABORADOR";
        public const string ALL_ROLES = "ADMIN, GESTOR, COLABORADOR";
        public const string ADMINISTRATIVE_ROLES = "ADMIN, GESTOR";

        [Display(Name = "Tipo")]
        public uint Id { get; set; }
        public string? Descricao { get; set; }
    }
}
