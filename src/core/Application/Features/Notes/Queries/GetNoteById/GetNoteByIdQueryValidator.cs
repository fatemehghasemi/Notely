using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Notely.Core.Application.Features.Notes.Queries.GetNoteById;

public class GetNoteByIdQueryValidator : AbstractValidator<GetNoteByIdQuery>
{
    public GetNoteByIdQueryValidator(IStringLocalizer<GetNoteByIdQueryValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(x => localizer["Id is required!"])
            .NotEqual(Guid.Empty)
            .WithMessage(x => localizer["Id must be a valid GUID!"]);
    }
}