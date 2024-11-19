using IntakerTestTask.Application.Abstractions.Repositories;
using IntakerTestTask.Application.Common.Models.Task;
using Microsoft.EntityFrameworkCore;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

namespace IntakerTestTask.Persistence.Implementations.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateTaskAsync(TaskModel task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task.Id;
    }

    public async Task<TaskModel> UpdateTaskStatusAsync(int id, TaskStatus status)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with Id = {id} not found");
        }

        task.Status = status;
        await _context.SaveChangesAsync();
        
        return task;
    }

    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
    {
        return await _context.Tasks.ToListAsync();
    }
}