using Shared.DTOs;

namespace Notely.Client.Services.Notes;

public interface INotesService
{
    Task<IEnumerable<NoteDto>> GetNotesAsync();
    Task<NoteDto?> GetNoteAsync(Guid id);
    Task<NoteDto> CreateNoteAsync(CreateNoteDto createNoteDto);
    Task<NoteDto> UpdateNoteAsync(Guid id, UpdateNoteDto updateNoteDto);
    Task DeleteNoteAsync(Guid id);
}