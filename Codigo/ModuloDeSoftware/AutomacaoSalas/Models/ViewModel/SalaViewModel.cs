﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class SalaViewModel
    {
        public SalaViewModel()
        {
            HardwaresSala = new List<HardwareDeSalaViewModel>();
            ConexaoSala = new List<ConexaoInternetSalaModel>();
        }
        [Display(Name = "Sala")]
        public SalaModel Sala { get; set; }
        [Display(Name = "Bloco")]
        public BlocoModel BlocoSala { get; set; }
        [Display(Name = "Hardwares")]
        public List<HardwareDeSalaViewModel> HardwaresSala { get; set; }
        public List<ConexaoInternetSalaModel> ConexaoSala{ get; set; }        
    }
}
