using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator(IStringLocalizer<GetCategoryByIdQueryValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["Category ID is required"]);
    }
}