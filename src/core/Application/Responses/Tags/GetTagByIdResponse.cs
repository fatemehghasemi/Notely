namespace Notely.Core.Application.Responses.Tags;

public class GetTagByIdResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int NotesCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}