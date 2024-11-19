using IntakerTestTask.Application.Common.Models.Messages;
using IntakerTestTask.Application.Common.Models.Task;
using Riok.Mapperly.Abstractions;

namespace IntakerTestTask.Application.Mappers.Messages;

[Mapper]
public static partial class CreateTaskMessageMapper
{
    public static partial CreateTaskMessage ToMessage(this TaskModel task);
}