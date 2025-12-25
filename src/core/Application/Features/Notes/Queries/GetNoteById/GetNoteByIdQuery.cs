using MediatR;

namespace Notely.Core.Application.Features.Notes.Queries.GetNoteById;

public record GetNoteByIdQuery(Guid Id) : IRequest<GetNoteByIdResponse?>;