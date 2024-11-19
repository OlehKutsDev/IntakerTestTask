using IntakerTestTask.Application.Abstractions.Handlers;
using Microsoft.Extensions.Hosting;

namespace DummyListener;

public class ServiceBusListener : BackgroundService
{
    private readonly IServiceBusHandler _serviceBusHandler;

    public ServiceBusListener(IServiceBusHandler serviceBusHandler)
    {
        _serviceBusHandler = serviceBusHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _serviceBusHandler.StartReceivingMessagesAsync(stoppingToken);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _serviceBusHandler.StopReceivingMessagesAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}