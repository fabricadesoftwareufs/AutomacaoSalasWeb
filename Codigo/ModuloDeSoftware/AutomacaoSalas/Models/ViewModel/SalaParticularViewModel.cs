using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class SalaParticularViewModel
    {
        public SalaParticularViewModel()
        {
            Responsaveis = new List<UsuarioModel>();
        }

        [Display(Name = "Código")]
        public uint Id { get; set; }
        [Display(Name = "Responsável")]
        public UsuarioModel Responsavel { get; set; }
        [Display(Name = "Sala")]
        public SalaModel SalaId { get; set; }
        public int BlocoId { get; set; }
        public List<UsuarioModel> Responsaveis { get; set; }
    }
}
