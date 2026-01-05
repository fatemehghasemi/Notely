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
}