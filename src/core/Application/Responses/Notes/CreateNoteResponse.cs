namespace Notely.Core.Application.Responses.Notes;

public class CreateNoteResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
