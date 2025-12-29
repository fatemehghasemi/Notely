using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(IStringLocalizer<UpdateCategoryCommandValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["Category ID is required"]);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localizer["Title is required"])
            .MaximumLength(100)
            .WithMessage(localizer["Title cannot exceed 100 characters"])
            .MinimumLength(2)
            .WithMessage(localizer["Title must be at least 2 characters"]);
    }
}