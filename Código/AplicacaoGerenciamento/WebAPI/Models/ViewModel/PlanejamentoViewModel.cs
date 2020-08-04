using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class PlanejamentoViewModel
    {
        public PlanejamentoModel Planejamento { get; set; }
        public SalaModel SalaId { get; set; }
        public UsuarioModel UsuarioId { get; set; }
        public int Id { get; set; }
        public string Periodo { get; set; }
        public string Horario { get; set; }
        public string DiaSemana { get; set; }
        public string Objetivo { get; set; }
    }
}
