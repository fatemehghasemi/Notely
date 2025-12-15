using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notely.Core.Domain.Entities;

namespace Notely.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
  public void Configure(EntityTypeBuilder<Tag> builder)
  {
    builder.HasIndex(t => t.Title).IsUnique();

    builder.HasMany(t => t.NoteTags)
           .WithOne(nt => nt.Tag)
           .HasForeignKey(nt => nt.TagId);
  }
}
