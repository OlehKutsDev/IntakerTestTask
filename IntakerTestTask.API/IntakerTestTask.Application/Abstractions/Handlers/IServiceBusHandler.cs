namespace IntakerTestTask.Application.Abstractions.Handlers;

public interface IServiceBusHandler
{
    Task SendMessageAsync<T>(T message, string action);
    Task StartReceivingMessagesAsync(CancellationToken cancellationToken);
    Task StopReceivingMessagesAsync(CancellationToken cancellationToken);

}