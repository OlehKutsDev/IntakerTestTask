using FluentValidation;
using IntakerTestTask.API.DTO.Task;

namespace IntakerTestTask.API.Validators.Task;

public class CreateTaskRequestDtoValidator : AbstractValidator<CreateTaskRequestDto>
{
    public CreateTaskRequestDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid value of TaskStatus enum");
    }
}