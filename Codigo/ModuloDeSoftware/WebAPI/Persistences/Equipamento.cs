using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Equipamento
    {
        public Equipamento()
        {
            Codigoinfravermelhos = new HashSet<Codigoinfravermelho>();
            Monitoramentos = new HashSet<Monitoramento>();
        }

        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Descricao { get; set; }
        public uint Sala { get; set; }
        public string TipoEquipamento { get; set; }
        public uint? HardwareDeSala { get; set; }

        public virtual Hardwaredesala HardwareDeSalaNavigation { get; set; }
        public virtual Sala SalaNavigation { get; set; }
        public virtual ICollection<Codigoinfravermelho> Codigoinfravermelhos { get; set; }
        public virtual ICollection<Monitoramento> Monitoramentos { get; set; }
    }
}
