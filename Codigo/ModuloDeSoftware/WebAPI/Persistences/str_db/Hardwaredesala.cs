using System;
using System.Collections.Generic;

namespace Persistence.str_db
{
    public partial class Hardwaredesala
    {
        public int Id { get; set; }
        public string Mac { get; set; }
        public int Sala { get; set; }
        public int TipoHardware { get; set; }
        public string Ip { get; set; }
        public string Uuid { get; set; }
        public string Token { get; set; }
    }
}
