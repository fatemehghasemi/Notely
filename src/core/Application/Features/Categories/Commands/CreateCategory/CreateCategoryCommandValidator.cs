using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(IStringLocalizer<CreateCategoryCommandValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localizer["Title is required"])
            .MaximumLength(100)
            .WithMessage(localizer["Title cannot exceed 100 characters"])
            .MinimumLength(2)
            .WithMessage(localizer["Title must be at least 2 characters"]);
    }
}