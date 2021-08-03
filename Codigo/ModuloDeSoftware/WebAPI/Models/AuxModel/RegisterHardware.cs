using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.AuxModel
{
    public class RegisterHardware
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "tipo_hardware_id")]
        public int TipoHardwareId { get; set; }
    }
}
