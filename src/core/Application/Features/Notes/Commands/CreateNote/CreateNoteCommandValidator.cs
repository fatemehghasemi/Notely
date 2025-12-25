using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Notes.Commands.CreateNote;

public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator(IStringLocalizer<CreateNoteCommandValidator> localizer)
    {
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
