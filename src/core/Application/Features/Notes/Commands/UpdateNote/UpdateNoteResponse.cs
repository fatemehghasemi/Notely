namespace Notely.Core.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}