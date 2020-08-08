using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class PlanejamentoViewModel
    {
        public PlanejamentoModel Planejamento { get; set; }
        public SalaModel SalaId { get; set; }
        public UsuarioModel UsuarioId { get; set; }
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Período")]
        public string Periodo { get; set; }
        [Display(Name = "Horário")]
        public string Horario { get; set; }
        [Display(Name = "Dia da Semana")]
        public string DiaSemana { get; set; }
        [Display(Name = "Objetivo")]
        public string Objetivo { get; set; }
    }
}
