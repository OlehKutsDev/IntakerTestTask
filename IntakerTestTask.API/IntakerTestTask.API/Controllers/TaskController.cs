using IntakerTestTask.API.Mappers.Task;
using IntakerTestTask.API.DTO.Task;
using IntakerTestTask.Application.Services.Task;
using Microsoft.AspNetCore.Mvc;

namespace IntakerTestTask.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _taskService.GetTasksAsync();
        return Ok(tasks);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskRequestDto request)
    {
        var taskId = await _taskService.CreateTaskAsync(request.ToTaskModel());
        return Ok(taskId);
    } 
    
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateTaskStatusAsync(int id, [FromBody] UpdateTaskStatusRequestDto request)
    {
        var task = await _taskService.UpdateTaskStatusAsync(id, request.Status);
        return Ok(task);
    }
}