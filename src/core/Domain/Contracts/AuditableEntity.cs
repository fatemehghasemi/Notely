namespace Notely.Core.Domain.Common;

public abstract class AuditableEntity<TId> : BaseEntity<TId>
{
    public new DateTime CreatedAt { get; set; }
    public new DateTime? UpdatedAt { get; set; }
}
