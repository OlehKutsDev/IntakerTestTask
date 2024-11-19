using IntakerTestTask.Application.Common.Models.Task;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

namespace IntakerTestTask.Application.Abstractions.Repositories;

public interface ITaskRepository
{
    Task<int> CreateTaskAsync(TaskModel task);
    Task<TaskModel> UpdateTaskStatusAsync(int id, TaskStatus status);
    Task<IEnumerable<TaskModel>> GetAllTasksAsync();  
}
