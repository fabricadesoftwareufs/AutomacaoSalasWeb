﻿
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class EquipamentoModel
    {
        public const string TIPO_CONDICIONADOR = "CONDICIONADOR";
        public const string TIPO_LUZES = "LUZES";

        public int Id { get; set; }
        
        public string? Modelo { get; set; }
        
        public string? Marca { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(1000, ErrorMessage = "Máximo são 1000 caracteres")]
        public string? Descricao { get; set; }
        public uint Sala { get; set; }
        public string TipoEquipamento { get; set; }
        public uint? HardwareDeSala { get; set; }

        public uint? IdModeloEquipamento { get; set; }
    }
}
