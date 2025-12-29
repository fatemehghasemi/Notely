namespace Notely.Core.Application.Responses.Categories;

public class CreateCategoryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}