using IntakerTestTask.Application.Common.Models.Task;
using IntakerTestTask.API.DTO.Task;
using Riok.Mapperly.Abstractions;

namespace IntakerTestTask.API.Mappers.Task;

[Mapper]
public static partial class TaskModelMapper
{
    public static partial TaskModel ToTaskModel(this CreateTaskRequestDto request);
}