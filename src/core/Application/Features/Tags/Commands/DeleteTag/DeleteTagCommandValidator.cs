using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Tags.Commands.DeleteTag;

public sealed class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator(IStringLocalizer<DeleteTagCommandValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["Tag ID is required"]);
    }
}