using PersonalNotesHub.Core.Domain.Common;

namespace PersonalNotesHub.Core.Domain.Entities;

public class Tag : BaseEntity<int>
{
  public string Name { get; set; } = string.Empty;
  public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}
