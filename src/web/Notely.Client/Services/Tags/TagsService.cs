using System.Net.Http.Json;
using Shared.DTOs;

namespace Notely.Client.Services.Tags;

public class TagsService : ITagsService
{
    private readonly HttpClient _httpClient;

    public TagsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TagDto>> GetTagsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<TagDto>>("api/tags");
        return response ?? Enumerable.Empty<TagDto>();
    }

    public async Task<TagDto?> GetTagAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<TagDto>($"api/tags/{id}");
    }

    public async Task<TagDto> CreateTagAsync(CreateTagDto createTagDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/tags", createTagDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TagDto>()
               ?? throw new InvalidOperationException("Failed to create tag");
    }

    public async Task<TagDto> UpdateTagAsync(Guid id, UpdateTagDto updateTagDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/tags/{id}", updateTagDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TagDto>()
               ?? throw new InvalidOperationException("Failed to update tag");
    }

    public async Task DeleteTagAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/tags/{id}");
        response.EnsureSuccessStatusCode();
    }
}
