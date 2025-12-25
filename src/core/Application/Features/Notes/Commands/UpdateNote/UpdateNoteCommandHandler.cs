using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Notes;

namespace Notely.Core.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, UpdateNoteResponse>
{
    private readonly INoteRepository _noteRepository;

    public UpdateNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<UpdateNoteResponse> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.Id, cancellationToken);
        if (note == null)
            throw new KeyNotFoundException($"Note with ID {request.Id} not found.");

        note.Title = request.Title;
        note.Content = request.Content;
        note.IsPinned = request.IsPinned;
        note.CategoryId = request.CategoryId;
        note.UpdatedAt = DateTime.UtcNow;

        var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);

        return new UpdateNoteResponse
        {
            Id = updatedNote.Id,
            Title = updatedNote.Title,
            UpdatedAt = updatedNote.UpdatedAt ?? updatedNote.CreatedAt
        };
    }
}
