using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class HardwareModel
    {
        public int Id { get; set; }
        public string MAC { get; set; }
        public int SalaId { get; set; }
        public int TipoHardwareId { get; set; }
    }
}
