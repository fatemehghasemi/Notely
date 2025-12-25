using MediatR;
using Notely.Core.Application.Responses.Notes;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Queries.GetNoteById;

public record GetNoteByIdQuery(Guid Id) : IRequest<Result<GetNoteByIdResponse?>>;