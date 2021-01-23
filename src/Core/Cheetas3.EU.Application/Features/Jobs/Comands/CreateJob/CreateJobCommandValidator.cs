﻿using FluentValidation;

namespace Cheetas3.EU.Application.Features.Jobs.Comands.CreateJob
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(v => v.Status)
                .NotEmpty();
        }
    }
}