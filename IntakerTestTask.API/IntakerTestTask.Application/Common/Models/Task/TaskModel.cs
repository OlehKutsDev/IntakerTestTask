namespace IntakerTestTask.Application.Common.Models.Task;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

public class TaskModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public TaskStatus Status { get; set; }
    
    public string? AssignedTo { get; set; }
}