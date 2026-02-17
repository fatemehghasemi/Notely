using Shared.DTOs;

namespace Notely.Client.Services.Categories;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryAsync(Guid id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto);
    Task DeleteCategoryAsync(Guid id);
}
