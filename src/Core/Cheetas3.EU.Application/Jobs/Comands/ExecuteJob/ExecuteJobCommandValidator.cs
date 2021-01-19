using Cheetas3.EU.Application.Jobs.Comands.ExecuteJob;
using FluentValidation;

namespace Cheetas3.EU.Application.Jobs.Comands.CreateJob
{
    public class ExecuteJobCommandValidator : AbstractValidator<ExecuteJobCommand>
    {
        public ExecuteJobCommandValidator()
        {
            RuleFor(v => v.JobId)
                .NotEmpty();
        }
    }
}
