using System;
using System.Collections.Generic;
using System.Text;

namespace Model.AuxModel
{
    public class SalaParticularAuxModel
    {
        public SalaParticularAuxModel()
        {
            Responsaveis = new List<UsuarioModel>();
        }

        public SalaParticularModel SalaParticular { get; set; }
        public List<UsuarioModel> Responsaveis { get; set; }
        public int BlocoSalas { get; set; }
        public int Organizacao { get; set; }
    }
}
