namespace PersonalNotesHub.Core.Domain.Common;

public abstract class AuditableEntity<TId> : BaseEntity<TId>
{
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; set; }

  public string? CreatedBy { get; set; }
  public string? UpdatedBy { get; set; }
}
