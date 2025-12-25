namespace Notely.Core.Application.Features.Notes.Commands.CreateNote;

public class CreateNoteResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}