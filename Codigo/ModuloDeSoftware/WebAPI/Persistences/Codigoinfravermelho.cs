using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Codigoinfravermelho
    {
        public int Id { get; set; }
        public int Equipamento { get; set; }
        public int Operacao { get; set; }
        public string Codigo { get; set; }

        public virtual Equipamento EquipamentoNavigation { get; set; }
        public virtual Operacao OperacaoNavigation { get; set; }
    }
}
