using Model.AuxModel;
using System.Collections.Generic;

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
