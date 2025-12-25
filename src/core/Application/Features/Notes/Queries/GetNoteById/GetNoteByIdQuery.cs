using MediatR;
using Notely.Core.Application.Responses.Notes;

namespace Notely.Core.Application.Features.Notes.Queries.GetNoteById;

public record GetNoteByIdQuery(Guid Id) : IRequest<GetNoteByIdResponse?>;