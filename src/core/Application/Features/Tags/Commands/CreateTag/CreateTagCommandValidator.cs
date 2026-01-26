using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Tags.Commands.CreateTag;

public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator(IStringLocalizer<CreateTagCommandValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localizer["Title is required"])
            .MaximumLength(50)
            .WithMessage(localizer["Title cannot exceed 50 characters"])
            .MinimumLength(2)
            .WithMessage(localizer["Title must be at least 2 characters"]);
    }
}