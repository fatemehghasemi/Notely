using MediatR;
using Notely.Core.Application.Responses.Notes;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Queries.GetAllNotes;

public record GetAllNotesQuery : IRequest<Result<GetAllNotesResponse>>;