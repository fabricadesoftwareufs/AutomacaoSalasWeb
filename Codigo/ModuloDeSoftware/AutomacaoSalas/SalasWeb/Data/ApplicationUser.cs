using Microsoft.AspNetCore.Identity;
using System;

namespace SalasWeb.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
    }
    
}
