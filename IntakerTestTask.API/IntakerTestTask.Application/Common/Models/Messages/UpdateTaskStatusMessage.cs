namespace IntakerTestTask.Application.Common.Models.Messages;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

public class UpdateTaskStatusMessage
{
    public int Id { get; set; }
    
    public TaskStatus Status { get; set; }
}