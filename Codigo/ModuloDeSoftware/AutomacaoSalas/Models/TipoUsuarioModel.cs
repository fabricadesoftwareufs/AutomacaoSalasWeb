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
        public const string ALL_ROLES2 = "ADMIN, GESTOR, COLABORADOR, PENDENTE";
        public const string ADMINISTRATIVE_ROLES = "ADMIN, GESTOR";
        public const string ROLE_PENDENTE = "PENDENTE";

        [Display(Name = "Tipo")]
        public uint Id { get; set; }
        
        [Display(Name = "Descrição")]
        [StringLength(45, ErrorMessage = "Máximo são 45 caracteres")]
        public string? Descricao { get; set; }
    }
}
