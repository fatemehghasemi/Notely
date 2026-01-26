using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Tags.Commands.UpdateTag;

public sealed class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator(IStringLocalizer<UpdateTagCommandValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["Tag ID is required"]);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localizer["Title is required"])
            .MaximumLength(50)
            .WithMessage(localizer["Title cannot exceed 50 characters"])
            .MinimumLength(2)
            .WithMessage(localizer["Title must be at least 2 characters"]);
    }
}