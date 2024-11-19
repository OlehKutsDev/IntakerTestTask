using FluentValidation;
using IntakerTestTask.API.DTO.Task;

namespace IntakerTestTask.API.Validators.Task;

public class UpdateTaskStatusRequestDtoValidator : AbstractValidator<UpdateTaskStatusRequestDto>
{
    public UpdateTaskStatusRequestDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid value of TaskStatus enum");
    }
}