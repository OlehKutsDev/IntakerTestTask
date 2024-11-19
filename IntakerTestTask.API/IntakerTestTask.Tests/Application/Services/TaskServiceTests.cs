using AutoFixture;
using IntakerTestTask.Application.Abstractions.Handlers;
using IntakerTestTask.Application.Abstractions.Repositories;
using IntakerTestTask.Application.Common.Models.Messages;
using IntakerTestTask.Application.Common.Models.Task;
using IntakerTestTask.Application.Services.Task;
using NSubstitute;
using NUnit.Framework;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

namespace IntakerTestTask.Tests.Application.Services;

[TestFixture]
public class TaskServiceTests
{
    private IServiceBusHandler _serviceBusHandler;
    private ITaskRepository _taskRepository;
    private TaskService _taskService;

    private Fixture _fixture;
    
    [SetUp]
    public void Setup()
    {
        _taskRepository = Substitute.For<ITaskRepository>();
        _serviceBusHandler = Substitute.For<IServiceBusHandler>();
        _taskService = new TaskService(_taskRepository, _serviceBusHandler);
        _fixture = new Fixture();
    }

    [Test]
    public async Task CreateTaskAsync_ShouldCallRepositoryAndServiceBus()
    {
        // Arrange
        var task = _fixture.Create<TaskModel>();
        _taskRepository.CreateTaskAsync(Arg.Any<TaskModel>()).ReturnsForAnyArgs(task.Id);
        
        // Act
        var id = await _taskService.CreateTaskAsync(task);

        // Assert
        await _taskRepository.Received(1).CreateTaskAsync(task);
        await _serviceBusHandler.Received(1).SendMessageAsync(
            Arg.Is<CreateTaskMessage>(msg => 
                msg.Id == task.Id &&
                msg.Name == task.Name && 
                msg.Description == task.Description &&
                msg.Status == task.Status &&
                msg.AssignedTo == task.AssignedTo),
            "Create");
        Assert.That(id, Is.EqualTo(task.Id));
    }

    [Test]
    public async Task UpdateTaskStatusAsync_ShouldCallRepositoryAndServiceBus()
    {
        // Arrange
        const int taskId = 1;
        const TaskStatus status = TaskStatus.Completed;
        var task = _fixture.Create<TaskModel>();
        task.Status = status;
        task.Id = taskId;
        
        _taskRepository.UpdateTaskStatusAsync(default, default).ReturnsForAnyArgs(task);

        // Act
        var actualTask = await _taskService.UpdateTaskStatusAsync(taskId, status);

        // Assert
        await _taskRepository.Received(1).UpdateTaskStatusAsync(taskId, status);
        await _serviceBusHandler.Received(1).SendMessageAsync(
            Arg.Is<UpdateTaskStatusMessage>(msg => 
                msg.Id == taskId && 
                msg.Status == status),
            "Update Status");
        Assert.Multiple(() =>
        {
            Assert.That(actualTask.Id, Is.EqualTo(taskId));
            Assert.That(actualTask.Status, Is.EqualTo(status));
        });
    }

    [Test]
    public async Task GetTasksAsync_ShouldReturnAllTasks()
    {
        // Arrange
        var tasks = _fixture.CreateMany<TaskModel>().ToList();
        _taskRepository.GetAllTasksAsync().Returns(tasks);

        // Act
        var result = await _taskService.GetTasksAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(tasks.Count));
        await _taskRepository.Received(1).GetAllTasksAsync();
    }
}