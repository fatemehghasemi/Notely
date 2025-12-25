using MediatR;
using Notely.Core.Application.Responses.Notes;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Commands.UpdateNote;

public record UpdateNoteCommand(
    Guid Id,
    string Title,
    string Content,
    bool IsPinned,
    Guid? CategoryId = null,
    List<string>? Tags = null
) : IRequest<Result<UpdateNoteResponse>>;
