using System.Text.Json.Serialization;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

namespace IntakerTestTask.API.DTO.Task;

public class CreateTaskRequestDto
{
    [JsonRequired]
    public string Name { get; set; } = string.Empty;
    
    [JsonRequired]
    public string Description { get; set; } = string.Empty;
    
    [JsonRequired]
    public TaskStatus Status { get; set; }
    
    public string? AssignedTo { get; set; }
}