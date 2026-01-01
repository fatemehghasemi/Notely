using MediatR;
using Shared.DTOs;
using Notely.Core.Application.Features.Notes.Commands.CreateNote;
using Notely.Core.Application.Features.Notes.Commands.UpdateNote;
using Notely.Core.Application.Features.Notes.Commands.DeleteNote;
using Notely.Core.Application.Features.Notes.Queries.GetAllNotes;
using Notely.Core.Application.Features.Notes.Queries.GetNoteById;

namespace Notely.Web.Services.AppServices;

public class NotesAppService : INotesAppService
{
    private readonly IMediator _mediator;

    public NotesAppService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<NoteDto>> GetAllNotesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllNotesQuery(), cancellationToken);
        
        if (!result.IsSuccess || result.Data?.Notes == null)
            return Enumerable.Empty<NoteDto>();

        return result.Data.Notes.Select(note => new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            IsPinned = note.IsPinned,
            CategoryId = note.CategoryId,
            CategoryName = note.CategoryName,
            Tags = note.Tags,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        });
    }

    public async Task<NoteDto?> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetNoteByIdQuery(id), cancellationToken);
        
        if (!result.IsSuccess || result.Data == null)
            return null;

        return new NoteDto
        {
            Id = result.Data.Id,
            Title = result.Data.Title,
            Content = result.Data.Content,
            IsPinned = result.Data.IsPinned,
            CategoryId = result.Data.CategoryId,
            CategoryName = result.Data.CategoryName,
            Tags = result.Data.Tags,
            CreatedAt = result.Data.CreatedAt,
            UpdatedAt = result.Data.UpdatedAt
        };
    }

    public async Task<Guid> CreateNoteAsync(CreateNoteDto dto, CancellationToken cancellationToken = default)
    {
        var command = new CreateNoteCommand(
            dto.Title,
            dto.Content,
            dto.IsPinned,
            dto.CategoryId,
            dto.Tags
        );

        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess || result.Data == null)
            throw new InvalidOperationException(result.ErrorMessage ?? "Failed to create note");

        return result.Data.Id;
    }

    public async Task UpdateNoteAsync(Guid id, UpdateNoteDto dto, CancellationToken cancellationToken = default)
    {
        var command = new UpdateNoteCommand(
            id,
            dto.Title,
            dto.Content,
            dto.IsPinned,
            dto.CategoryId,
            dto.Tags
        );

        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
            throw new InvalidOperationException(result.ErrorMessage ?? "Failed to update note");
    }

    public async Task DeleteNoteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteNoteCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
            throw new InvalidOperationException(result.ErrorMessage ?? "Failed to delete note");
    }
}