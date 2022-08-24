using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence
{
    public partial class Logrequest
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public string Input { get; set; }
        public string StatusCode { get; set; }
        public string Origin { get; set; }
    }
}
