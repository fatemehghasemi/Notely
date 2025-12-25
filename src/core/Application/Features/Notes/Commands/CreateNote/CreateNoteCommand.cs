using MediatR;
using Notely.Core.Application.Responses.Notes;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Commands.CreateNote;

public record CreateNoteCommand(
    string Title,
    string Content,
    bool IsPinned = false,
    Guid? CategoryId = null,
    List<string>? Tags = null
) : IRequest<Result<CreateNoteResponse>>;
