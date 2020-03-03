using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class BlocoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public OrganizacaoModel OrganizacaoId { get; set; }
    }
}
