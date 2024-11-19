using IntakerTestTask.Application.Abstractions.Handlers;
using IntakerTestTask.Application.Abstractions.Repositories;
using IntakerTestTask.Application.Common.Models.Messages;
using IntakerTestTask.Application.Common.Models.Task;
using IntakerTestTask.Application.Mappers.Messages;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

namespace IntakerTestTask.Application.Services.Task;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IServiceBusHandler _serviceBusHandler;
    
    public TaskService(ITaskRepository taskRepository, IServiceBusHandler serviceBusHandler)
    {
        _taskRepository = taskRepository;
        _serviceBusHandler = serviceBusHandler;
    }
    
    public async Task<int> CreateTaskAsync(TaskModel task)
    {
        var taskId = await _taskRepository.CreateTaskAsync(task);
        await _serviceBusHandler.SendMessageAsync(task.ToMessage(), "Create");
        return taskId;
    }

    public async Task<TaskModel> UpdateTaskStatusAsync(int id, TaskStatus status)
    {
        var task = await _taskRepository.UpdateTaskStatusAsync(id, status);
        await _serviceBusHandler.SendMessageAsync(new UpdateTaskStatusMessage { Id = id, Status = status }, "Update Status");
        
        return task;
    }

    public async Task<IEnumerable<TaskModel>> GetTasksAsync()
    {
        return await _taskRepository.GetAllTasksAsync();;
    }
}