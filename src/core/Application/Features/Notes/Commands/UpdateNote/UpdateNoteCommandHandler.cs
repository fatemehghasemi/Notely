using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Notes;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Result<UpdateNoteResponse>>
{
    private readonly INoteRepository _noteRepository;

    public UpdateNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Result<UpdateNoteResponse>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var note = await _noteRepository.GetByIdAsync(request.Id, cancellationToken);
            if (note == null)
            {
                return Result<UpdateNoteResponse>.Failure($"Note with ID {request.Id} not found.");
            }

            note.Title = request.Title;
            note.Content = request.Content;
            note.IsPinned = request.IsPinned;
            note.CategoryId = request.CategoryId;
            note.UpdatedAt = DateTime.UtcNow;

            var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);

            var response = new UpdateNoteResponse
            {
                Id = updatedNote.Id,
                Title = updatedNote.Title,
                UpdatedAt = updatedNote.UpdatedAt ?? updatedNote.CreatedAt
            };

            return Result<UpdateNoteResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<UpdateNoteResponse>.Failure($"Failed to update note: {ex.Message}");
        }
    }
}
