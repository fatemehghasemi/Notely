using Notely.Core.Domain.Common;

namespace Notely.Core.Domain.Entities;

public sealed class Tag : AuditableEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}
