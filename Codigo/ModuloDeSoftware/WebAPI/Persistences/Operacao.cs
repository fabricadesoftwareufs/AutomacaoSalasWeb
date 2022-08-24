using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Operacao
    {
        public Operacao()
        {
            Codigoinfravermelhos = new HashSet<Codigoinfravermelho>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<Codigoinfravermelho> Codigoinfravermelhos { get; set; }
    }
}
