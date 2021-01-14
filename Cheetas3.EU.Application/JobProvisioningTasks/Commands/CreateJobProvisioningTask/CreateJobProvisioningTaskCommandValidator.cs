using FluentValidation;

namespace Cheetas3.EU.Application.JobProvisioningTasks.Commands.CreateTask
{
    public class CreateJobProvisioningTaskCommandValidator : AbstractValidator<CreateJobProvisioningTaskCommand>
    {
        public CreateJobProvisioningTaskCommandValidator()
        {
            RuleFor(v => v.Status)
                .NotEmpty();
            RuleFor(v => v.FileTimeSpan)
                .NotEmpty();
        }
    }
}