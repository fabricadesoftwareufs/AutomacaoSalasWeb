namespace Service.Interface
{
    public interface IMqttService
    {
        bool PublishMessage(string topic, string payload);
    }
}
