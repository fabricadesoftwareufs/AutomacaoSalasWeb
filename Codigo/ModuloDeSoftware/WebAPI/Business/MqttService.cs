using Microsoft.Extensions.Options;
using Model.MqttOptions;
using Service.Connections;
using Service.Interface;
using System;

namespace Service
{
    public class MqttService : IMqttService
    {
        private readonly MqttConnector _mqttConnector;
        private readonly IOptions<MqttOptions> _mqttOptions;

        public MqttService(IOptions<MqttOptions> mqttOptions)
        {
            if(mqttOptions.Value == null || mqttOptions.Value.BrokerAddress == null)
               throw new ArgumentNullException("Não foi possível estabelecer conexão com o servidor. Tente novamente mais tarde!.");
            
            _mqttOptions = mqttOptions;
            _mqttConnector = new MqttConnector(_mqttOptions.Value);
        }

        public void PublishMessage(string topic, string payload)
        {
            _mqttConnector.PublishMessage(topic, payload);
        }
    }
}
