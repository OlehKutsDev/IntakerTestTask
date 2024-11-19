namespace DummyServiceBus.Handlers;

public interface IServiceBusHandler
{
    void SendMessage<T>(T message, string action);
    void ReceiveMessage();
}