using System.Net.Http.Json;
using Shared.DTOs;

namespace Notely.Client.Services.Notes;

public class NotesService : INotesService
{
    private readonly HttpClient _httpClient;

    public NotesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<NoteDto>> GetNotesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<NoteDto>>("api/notes");
        return response ?? Enumerable.Empty<NoteDto>();
    }

    public async Task<NoteDto?> GetNoteAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<NoteDto>($"api/notes/{id}");
    }

    public async Task<NoteDto> CreateNoteAsync(CreateNoteDto createNoteDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/notes", createNoteDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<NoteDto>() ?? throw new InvalidOperationException("Failed to create note");
    }

    public async Task<NoteDto> UpdateNoteAsync(Guid id, UpdateNoteDto updateNoteDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/notes/{id}", updateNoteDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<NoteDto>() ?? throw new InvalidOperationException("Failed to update note");
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/notes/{id}");
        response.EnsureSuccessStatusCode();
    }
}