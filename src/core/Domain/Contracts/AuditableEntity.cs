namespace Notely.Core.Domain.Common;

public abstract class AuditableEntity<TId> : BaseEntity<TId>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
