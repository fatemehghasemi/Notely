using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Tags.Queries.GetTagById;

public sealed class GetTagByIdQueryValidator : AbstractValidator<GetTagByIdQuery>
{
    public GetTagByIdQueryValidator(IStringLocalizer<GetTagByIdQueryValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["Tag ID is required"]);
    }
}