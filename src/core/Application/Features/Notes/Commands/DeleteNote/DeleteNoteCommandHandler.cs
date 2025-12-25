using MediatR;
using Notely.Core.Application.Interfaces.Repositories;

namespace Notely.Core.Application.Features.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
{
    private readonly INoteRepository _noteRepository;

    public DeleteNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        return await _noteRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
