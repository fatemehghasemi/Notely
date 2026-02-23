using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Notes.Commands.CreateNote;

public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator(IStringLocalizer<CreateNoteCommandValidator> localizer)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(x => localizer["Title is required!"])
            .Must(t => !string.IsNullOrWhiteSpace(t))
            .WithMessage(x => localizer["Title cannot be whitespace!"])
            .MaximumLength(200)
            .WithMessage(x => localizer["Title must not exceed 200 characters!"]);

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(x => localizer["Content is required!"])
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage(x => localizer["Content cannot be whitespace!"]);

        RuleFor(x => x.CategoryId)
            .NotNull()
            .WithMessage(x => localizer["CategoryId is required!"])
            .NotEqual(Guid.Empty)
            .WithMessage(x => localizer["CategoryId must be a valid GUID!"]);

        RuleFor(x => x.Tags)
            .Must(tags => tags == null || tags.Distinct(StringComparer.OrdinalIgnoreCase).Count() == tags.Count)
            .WithMessage(x => localizer["Duplicate tags are not allowed!"]);

        RuleForEach(x => x.Tags)
            .Must(tag => !string.IsNullOrWhiteSpace(tag))
            .WithMessage(x => localizer["Tag names cannot be empty or whitespace!"]);
    }
}
