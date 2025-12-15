using PersonalNotesHub.Core.Domain.Common;

namespace PersonalNotesHub.Core.Domain.Entities;

public class Category : BaseEntity<int>
{
  public string Name { get; set; } = string.Empty;
  public ICollection<Note> Notes { get; set; } = new HashSet<Note>();
}

