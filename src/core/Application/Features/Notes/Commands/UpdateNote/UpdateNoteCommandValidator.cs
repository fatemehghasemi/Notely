using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator(IStringLocalizer<UpdateNoteCommandValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(x => localizer["Id is required!"]);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(x => localizer["Title is required!"])
            .MaximumLength(200)
            .WithMessage(x => localizer["Title must not exceed 200 characters!"]);

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(x => localizer["Content is required!"]);

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .When(x => x.CategoryId.HasValue)
            .WithMessage(x => localizer["CategoryId must be a valid GUID!"]);

        RuleFor(x => x.Tags)
            .Must(tags => tags == null || tags.All(tag => !string.IsNullOrWhiteSpace(tag)))
            .WithMessage(x => localizer["Tag names cannot be empty or whitespace!"]);
    }
}
