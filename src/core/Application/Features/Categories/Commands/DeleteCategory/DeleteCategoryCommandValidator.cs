using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator(IStringLocalizer<DeleteCategoryCommandValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["Category ID is required"]);
    }
}