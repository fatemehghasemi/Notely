using Shared.DTOs;

namespace Notely.Web.Services.AppServices;

public interface INotesAppService
{
    Task<IEnumerable<NoteDto>> GetAllNotesAsync(CancellationToken cancellationToken = default);
    Task<NoteDto?> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateNoteAsync(CreateNoteDto dto, CancellationToken cancellationToken = default);
    Task UpdateNoteAsync(Guid id, UpdateNoteDto dto, CancellationToken cancellationToken = default);
    Task DeleteNoteAsync(Guid id, CancellationToken cancellationToken = default);
}