using Shared.DTOs;

namespace Notely.Client.Services.Categories;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
}