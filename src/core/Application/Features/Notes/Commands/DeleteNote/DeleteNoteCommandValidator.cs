using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Notes.Commands.DeleteNote;

public class DeleteNoteCommandValidator : AbstractValidator<DeleteNoteCommand>
{
    public DeleteNoteCommandValidator(IStringLocalizer<DeleteNoteCommandValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(x => localizer["Id is required!"])
            .NotEqual(Guid.Empty)
            .WithMessage(x => localizer["Id must be a valid GUID!"]);
    }
}
