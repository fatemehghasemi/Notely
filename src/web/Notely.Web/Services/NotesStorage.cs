namespace Notely.Web.Services;

public static class NotesStorage
{
    private static List<NoteDto> _notes = new();

    public static List<NoteDto> GetAllNotes() => _notes.ToList();

    public static void AddNote(NoteDto note)
    {
        _notes.Add(note);
    }

    public static void UpdateNote(NoteDto note)
    {
        var existing = _notes.FirstOrDefault(n => n.Id == note.Id);
        if (existing != null)
        {
            existing.Title = note.Title;
            existing.Content = note.Content;
        }
    }

    public static void DeleteNote(Guid id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null)
        {
            _notes.Remove(note);
        }
    }

    public static NoteDto? GetNote(Guid id)
    {
        return _notes.FirstOrDefault(n => n.Id == id);
    }
}

public class NoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}