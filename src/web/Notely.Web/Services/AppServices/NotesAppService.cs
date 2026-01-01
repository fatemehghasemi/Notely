using Shared.DTOs;

namespace Notely.Web.Services.AppServices;

public class NotesAppService : INotesAppService
{
    public NotesAppService()
    {
    }

    public Task<IEnumerable<NoteDto>> GetAllNotesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<NoteDto?> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateNoteAsync(CreateNoteDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNoteAsync(Guid id, UpdateNoteDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteNoteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}