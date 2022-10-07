using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IMqttService
    {
        void PublishMessage(string topic, string payload);
    }
}
