using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModel
{
    public class SalaViewModel
    {
        [Display(Name = "Sala")]
        public SalaModel Sala { get; set; }
        [Display(Name = "Bloco")]
        public BlocoModel BlocoSala { get; set; }
        [Display(Name = "Hardwares")]
        public List<HardwareDeSalaViewModel> HardwaresSala { get; set; }
    }
}
