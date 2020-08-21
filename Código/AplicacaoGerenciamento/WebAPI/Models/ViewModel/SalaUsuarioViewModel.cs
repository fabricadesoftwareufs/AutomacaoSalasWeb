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
            SalasUsuario = new List<SalaUsuarioAuxModel>();
        }

        public List<SalaUsuarioAuxModel> SalasUsuario { get; set; }
    }
}
