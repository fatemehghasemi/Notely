using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notely.Core.Domain.Entities;

namespace Notely.Infrastructure.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasOne(n => n.Category)
               .WithMany(c => c.Notes)
               .HasForeignKey(n => n.CategoryId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(n => n.NoteTags)
               .WithOne(nt => nt.Note)
               .HasForeignKey(nt => nt.NoteId);

        builder.Property(n => n.Color)
               .HasConversion<int>();
    }
}
