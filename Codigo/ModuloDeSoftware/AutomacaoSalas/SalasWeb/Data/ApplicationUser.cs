using Microsoft.AspNetCore.Identity;

namespace SalasWeb.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Cpf { get; set; }
    }
    
}
