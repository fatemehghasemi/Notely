using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Result>
{
    private readonly INoteRepository _noteRepository;

    public DeleteNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Result> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _noteRepository.ExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                return Result.Failure($"Note with ID {request.Id} not found.");
            }

            var deleted = await _noteRepository.DeleteAsync(request.Id, cancellationToken);
            if (!deleted)
            {
                return Result.Failure("Failed to delete note.");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete note: {ex.Message}");
        }
    }
}
