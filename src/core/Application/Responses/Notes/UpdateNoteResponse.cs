namespace Notely.Core.Application.Responses.Notes;

public class UpdateNoteResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}