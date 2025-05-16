using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;

namespace Model
{
    public class MonitoramentoModel
    {
        public int Id { get; set; }
        public int IdEquipamento { get; set; }
        public int IdOperacao { get; set; }
        public DateTime DataHora { get; set; } = DateTime.Now;
        public uint IdUsuario { get; set; }  // Alterado para uint conforme scaffold
        public int Temperatura { get; set; } = 0;

        // Navegações para objetos relacionados - nomes ajustados conforme scaffold
        public virtual EquipamentoModel IdEquipamentoNavigation { get; set; } = null!;
        public virtual OperacaoModel IdOperacaoNavigation { get; set; } = null!;
        public virtual UsuarioModel IdUsuarioNavigation { get; set; } = null!;
    }
}