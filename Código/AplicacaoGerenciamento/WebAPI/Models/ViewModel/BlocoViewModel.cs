using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class BlocoViewModel
    {

        public BlocoViewModel()
        {
            Hardwares = new List<HardwareDeBlocoModel>();
        }

        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Display(Name = "Organização")]
        public OrganizacaoModel OrganizacaoId { get; set; }
        [Display(Name = "Hardwares")]
        public List<HardwareDeBlocoModel> Hardwares { get; set; }
    }
}
