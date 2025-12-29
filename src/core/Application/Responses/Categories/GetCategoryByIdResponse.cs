namespace Notely.Core.Application.Responses.Categories;

public class GetCategoryByIdResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int NotesCount { get; set; }
    public List<CategoryNoteItem> Notes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CategoryNoteItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}