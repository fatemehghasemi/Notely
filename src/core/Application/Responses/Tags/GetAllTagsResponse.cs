namespace Notely.Core.Application.Responses.Tags;

public class GetAllTagsResponse
{
    public List<TagResponse> Tags { get; set; } = new();
}

public class TagResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}