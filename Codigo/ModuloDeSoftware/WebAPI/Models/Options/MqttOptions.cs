using System;

namespace Model.MqttOptions
{
    public class MqttOptions
    {
        public Guid ClientId { get; set; }

        public string BrokerAddress { get; set; } = string.Empty;

        public int Port { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
