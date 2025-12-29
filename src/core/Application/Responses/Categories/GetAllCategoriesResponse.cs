namespace Notely.Core.Application.Responses.Categories;

public class GetAllCategoriesResponse
{
    public List<CategoryItem> Categories { get; set; } = new();
}

public class CategoryItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int NotesCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}