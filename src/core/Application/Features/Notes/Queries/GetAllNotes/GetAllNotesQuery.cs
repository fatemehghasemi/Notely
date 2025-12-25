using MediatR;
using Notely.Core.Application.Responses.Notes;

namespace Notely.Core.Application.Features.Notes.Queries.GetAllNotes;

public record GetAllNotesQuery : IRequest<GetAllNotesResponse>;