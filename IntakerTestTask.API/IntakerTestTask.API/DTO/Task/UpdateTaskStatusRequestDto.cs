using System.Text.Json.Serialization;
using TaskStatus = IntakerTestTask.Application.Common.Enums.Task.TaskStatus;

namespace IntakerTestTask.API.DTO.Task;

public class UpdateTaskStatusRequestDto
{
    [JsonRequired]
    public TaskStatus Status { get; set; }
}