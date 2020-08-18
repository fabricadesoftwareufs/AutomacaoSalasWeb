using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class MonitoramentoModel
    {
        public int Id { get; set; }
        public bool Luzes { get; set; }
        public bool ArCondicionado { get; set; }
        public int SalaId { get; set; }
    }
}
