namespace Notely.Core.Application.Responses.Tags;

public class CreateTagResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}