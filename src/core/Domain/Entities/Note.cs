using PersonalNotesHub.Core.Domain.Common;
using PersonalNotesHub.Core.Domain.Enum;

namespace PersonalNotesHub.Core.Domain.Entities;

public class Note : AuditableEntity<Guid>
{
  public string Title { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public NoteColor Color { get; set; } = NoteColor.None;

  public bool IsPinned { get; set; }

  public int? CategoryId { get; set; }
  public Category? Category { get; set; }

  public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}

