using Notely.Core.Domain.Common;
using Notely.Core.Domain.Enums;

namespace Notely.Core.Domain.Entities;

public sealed class Note : AuditableEntity<Guid>
{
  public string Title { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public NoteColor Color { get; set; } = NoteColor.None;

  public bool IsPinned { get; set; }

  public Guid? CategoryId { get; set; }
  public Category? Category { get; set; }

  public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}

