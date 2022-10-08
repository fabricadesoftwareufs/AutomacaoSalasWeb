using Model.MqttOptions;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Service.Connections
{
    public class MqttConnector : MqttClient
    {
        private readonly MqttOptions _mqttOptions;
        private readonly int SUCCESS_CODE = 0;

        public MqttConnector(MqttOptions mqttOptions) 
            : base(mqttOptions.BrokerAddress, mqttOptions.Port, true, null, null, MqttSslProtocols.TLSv1_2) 
        {
            _mqttOptions = mqttOptions;
        }

        public bool PublishMessage(string topic, string message)
        {
            var connected = Connect(_mqttOptions.ClientId.ToString(), _mqttOptions.UserId, _mqttOptions.Password);

            if (connected != SUCCESS_CODE)
                return false;
            
            var published = Publish($"esp/{topic}", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            
            Disconnect();

            return (published == SUCCESS_CODE);
        }
    }
}
