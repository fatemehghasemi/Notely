using Microsoft.EntityFrameworkCore;
using Notely.Core.Domain.Common;
using Notely.Core.Domain.Entities;

namespace Notely.Infrastructure.Persistence;

public class BlazorNotelyContext : DbContext
{
  public BlazorNotelyContext(DbContextOptions<BlazorNotelyContext> options)
      : base(options) { }

  // DbSets
  public DbSet<Note> Notes => Set<Note>();
  public DbSet<Category> Categories => Set<Category>();
  public DbSet<Tag> Tags => Set<Tag>();
  public DbSet<NoteTag> NoteTags => Set<NoteTag>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    ApplyGlobalConventions(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlazorNotelyContext).Assembly);
  }

  private void ApplyGlobalConventions(ModelBuilder modelBuilder)
  {
    foreach (var property in modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
    {
      property.SetColumnType("decimal(18,2)");
    }

    modelBuilder.Entity<Note>()
        .Property(n => n.Color)
        .HasConversion<int>()
        .IsRequired();
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var entry in ChangeTracker.Entries<AuditableEntity<Guid>>())
    {
      switch (entry.State)
      {
        case EntityState.Added:
          entry.Entity.CreatedAt = DateTime.UtcNow;
          break;

        case EntityState.Modified:
          entry.Entity.UpdatedAt = DateTime.UtcNow;
          break;
      }
    }

    return await base.SaveChangesAsync(cancellationToken);
  }
}
