using System.Text;
using Azure.Messaging.ServiceBus;
using IntakerTestTask.Application.Abstractions.Handlers;
using Newtonsoft.Json;

namespace IntakerTestTask.Infrastructure.Implementations.Handlers;

public class ServiceBusHandler : IServiceBusHandler
{
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusProcessor _processor;

    public ServiceBusHandler(ServiceBusClient serviceBusClient, string topicName, string subscriptionName)
    {
        _sender = serviceBusClient.CreateSender(topicName);
        _processor = serviceBusClient.CreateProcessor(topicName, subscriptionName);
    }

    public async Task SendMessageAsync<T>(T message, string action)
    {
        var messageBody = JsonConvert.SerializeObject(message);
        var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody))
        {
            ApplicationProperties =
            {
                { "Action", action }
            }
        };
        
        await _sender.SendMessageAsync(serviceBusMessage);
    }

    public async Task StartReceivingMessagesAsync(CancellationToken cancellationToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;

        await _processor.StartProcessingAsync(cancellationToken);
    }
    
    public async Task StopReceivingMessagesAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await _processor.CloseAsync(cancellationToken);
    }
    
    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        
        try
        {
            Console.WriteLine($"Received message: {message.Body} - Action: {message.ApplicationProperties["Action"]}");
            await args.CompleteMessageAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
            await args.AbandonMessageAsync(message);
        }
    }

    private static Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error occurred: {args.Exception.Message}");
        return Task.CompletedTask;
    }
}