namespace Notely.Core.Application.Responses.Tags;

public class GetAllTagsResponse
{
    public List<TagItem> Tags { get; set; } = new();
}

public class TagItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}