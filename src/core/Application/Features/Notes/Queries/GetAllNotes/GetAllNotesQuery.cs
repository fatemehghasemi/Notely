using MediatR;

namespace Notely.Core.Application.Features.Notes.Queries.GetAllNotes;

public record GetAllNotesQuery : IRequest<GetAllNotesResponse>;