using System;
using System.Collections.Generic;



namespace Persistence
{
    public partial class Equipamento
    {
        public Equipamento()
        {
            Codigoinfravermelho = new HashSet<Codigoinfravermelho>();
            Monitoramento = new HashSet<Monitoramento>();
        }

        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Descricao { get; set; }
        public int Sala { get; set; }
        public string TipoEquipamento { get; set; }
        public int? HardwareDeSala { get; set; }

        public virtual Hardwaredesala HardwareDeSalaNavigation { get; set; }
        public virtual Sala SalaNavigation { get; set; }
        public virtual ICollection<Codigoinfravermelho> Codigoinfravermelho { get; set; }
        public virtual ICollection<Monitoramento> Monitoramento { get; set; }
    }
}
