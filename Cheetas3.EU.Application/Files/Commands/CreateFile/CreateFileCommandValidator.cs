using FluentValidation;

namespace Cheetas3.EU.Application.Files.Commands.CreateFile
{
    public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
    {
        public CreateFileCommandValidator()
        {
            RuleFor(v => v.Status)
                .NotEmpty();
            RuleFor(v => v.StartTime)
                .NotEmpty();
            RuleFor(v => v.EndTime)
                .NotEmpty();
            RuleFor(v => v).Must(v => v.EndTime == default ||
                                 v.StartTime == default ||
                                 v.EndTime > v.StartTime)
                                 .WithMessage("EndTime must be greater than StartTime");
        }
    }
}