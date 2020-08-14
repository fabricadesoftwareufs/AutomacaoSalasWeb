using Model.AuxModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModel
{
    public class SalaUsuarioViewModel
    {
        public SalaUsuarioViewModel()
        {
            SalasExclusivas = new List<SalaUsuarioAuxModel>();
            Reservadas = new List<SalaUsuarioAuxModel>();
        }

        public List<SalaUsuarioAuxModel> SalasExclusivas { get; set; }
        public List<SalaUsuarioAuxModel> Reservadas { get; set; }
    }
}
