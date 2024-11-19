using System.Text;
using AutoFixture;
using Azure.Messaging.ServiceBus;
using IntakerTestTask.Application.Common.Models.Messages;
using IntakerTestTask.Infrastructure.Implementations.Handlers;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace IntakerTestTask.Tests.Infrastructure.Handlers;

[TestFixture]
public class ServiceBusHandlerTests
{
    private ServiceBusProcessor _mockProcessor;
    private ServiceBusSender _mockSender;
    private ServiceBusClient _mockServiceBusClient;
    private ServiceBusHandler _serviceBusHandler;
    
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockServiceBusClient = Substitute.For<ServiceBusClient>();
        _mockSender = Substitute.For<ServiceBusSender>();
        _mockProcessor = Substitute.For<ServiceBusProcessor>();

        _mockServiceBusClient.CreateSender(Arg.Any<string>()).Returns(_mockSender);
        _mockServiceBusClient.CreateProcessor(Arg.Any<string>(), Arg.Any<string>()).Returns(_mockProcessor);

        _serviceBusHandler = new ServiceBusHandler(_mockServiceBusClient, "TestTopic", "TestSubscription");
        
        _fixture = new Fixture();
    }

    [Test]
    public async Task SendMessageAsync_ShouldSendMessageToServiceBus()
    {
        // Arrange
        const string action = "Create";
        var message = _fixture.Create<CreateTaskMessage>();

        // Act
        await _serviceBusHandler.SendMessageAsync(message, action);

        // Assert
        await _mockSender.Received(1).SendMessageAsync(Arg.Is<ServiceBusMessage>(msg =>
            Encoding.UTF8.GetString(msg.Body.ToArray()) == JsonConvert.SerializeObject(message) &&
            msg.ApplicationProperties["Action"].ToString() == action));
    }

    [Test]
    public async Task StartReceivingMessagesAsync_ShouldStartMessageProcessing()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Act
        await _serviceBusHandler.StartReceivingMessagesAsync(cancellationToken);

        // Assert
        await _mockProcessor.Received(1).StartProcessingAsync(cancellationToken);
    }

    [Test]
    public async Task StopReceivingMessagesAsync_ShouldStopMessageProcessing()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Act
        await _serviceBusHandler.StopReceivingMessagesAsync(cancellationToken);

        // Assert
        await _mockProcessor.Received(1).StopProcessingAsync(cancellationToken);
        await _mockProcessor.Received(1).CloseAsync(cancellationToken);
    }
}