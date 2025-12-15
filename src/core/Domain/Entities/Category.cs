using Notely.Core.Domain.Common;

namespace Notely.Core.Domain.Entities;

public sealed class Category : BaseEntity<Guid>
{
  public string Title { get; set; } = string.Empty;
  public ICollection<Note> Notes { get; set; } = new HashSet<Note>();
}

