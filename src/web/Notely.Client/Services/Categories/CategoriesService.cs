using System.Net.Http.Json;
using Shared.DTOs;

namespace Notely.Client.Services.Categories;

public class CategoriesService : ICategoriesService
{
    private readonly HttpClient _httpClient;

    public CategoriesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>("api/categories");
        return response ?? Enumerable.Empty<CategoryDto>();
    }

    public async Task<CategoryDto?> GetCategoryAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<CategoryDto>($"api/categories/{id}");
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/categories", createCategoryDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CategoryDto>()
               ?? throw new InvalidOperationException("Failed to create category");
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{id}", updateCategoryDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CategoryDto>()
               ?? throw new InvalidOperationException("Failed to update category");
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/categories/{id}");
        response.EnsureSuccessStatusCode();
    }
}
