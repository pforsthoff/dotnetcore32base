using Cheetas3.EU.Application.Features.Jobs.Comands.ExecuteJob;
using FluentValidation;

namespace Cheetas3.EU.Application.Features.Jobs.Comands.CreateJob
{
    public class ExecuteJobCommandValidator : AbstractValidator<ExecuteJobCommand>
    {
        public ExecuteJobCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}
