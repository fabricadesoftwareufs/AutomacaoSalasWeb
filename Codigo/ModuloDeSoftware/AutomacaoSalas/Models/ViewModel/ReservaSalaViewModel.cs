using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class ReservaSalaViewModel
    {
        public HorarioSalaModel HorarioSalaModel { get; set; }
        public OrganizacaoModel? OrganizacaoModel { get; set; }
        public BlocoModel? BlocoModel { get; set; }
    }
}
