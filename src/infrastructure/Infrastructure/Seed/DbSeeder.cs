using Microsoft.EntityFrameworkCore;
using Notely.Core.Domain.Entities;
using Notely.Core.Domain.Enums;
using Notely.Infrastructure.Persistence;

namespace Infrastructure.Seed;

public static class DbSeeder
{
  public static async Task SeedAsync(BlazorNotelyContext context)
  {
    await context.Database.MigrateAsync();

    if (!context.Categories.Any())
    {
      context.Categories.AddRange(
          new Category { Id = Guid.NewGuid(), Title = "Work" },
          new Category { Id = Guid.NewGuid(), Title = "Personal" },
          new Category { Id = Guid.NewGuid(), Title = "Shopping" },
          new Category { Id = Guid.NewGuid(), Title = "Health" }
      );
      await context.SaveChangesAsync();
    }

    if (!context.Tags.Any())
    {
      context.Tags.AddRange(
          new Tag { Id = Guid.NewGuid(), Title = "Urgent" },
          new Tag { Id = Guid.NewGuid(), Title = "Important" },
          new Tag { Id = Guid.NewGuid(), Title = "Optional" },
          new Tag { Id = Guid.NewGuid(), Title = "Follow-up" }
      );
      await context.SaveChangesAsync();
    }

    if (!context.Notes.Any())
    {
      var workCategory = await context.Categories.FirstAsync(c => c.Title == "Work");
      var personalCategory = await context.Categories.FirstAsync(c => c.Title == "Personal");

      context.Notes.AddRange(
          new Note
          {
            Id = Guid.NewGuid(),
            Title = "Prepare monthly report",
            Content = "Compile the sales and finance data for this month and send to management.",
            Color = NoteColor.Red,
            CategoryId = workCategory.Id
          },
          new Note
          {
            Id = Guid.NewGuid(),
            Title = "Buy groceries",
            Content = "Milk, Eggs, Bread, and Fresh Vegetables for the week.",
            Color = NoteColor.Blue,
            CategoryId = personalCategory.Id
          },
          new Note
          {
            Id = Guid.NewGuid(),
            Title = "Doctor appointment",
            Content = "Annual health checkup at 3 PM on Thursday.",
            Color = NoteColor.Green,
            CategoryId = personalCategory.Id
          }
      );
      await context.SaveChangesAsync();
    }

    if (!context.NoteTags.Any())
    {
      var notes = await context.Notes.ToListAsync();
      var urgentTag = await context.Tags.FirstAsync(t => t.Title == "Urgent");
      var importantTag = await context.Tags.FirstAsync(t => t.Title == "Important");

      var noteTags = new List<NoteTag>
    {
        new NoteTag { NoteId = notes[0].Id, TagId = importantTag.Id }, 
        new NoteTag { NoteId = notes[0].Id, TagId = urgentTag.Id },
        new NoteTag { NoteId = notes[1].Id, TagId = importantTag.Id },
        new NoteTag { NoteId = notes[2].Id, TagId = urgentTag.Id }    
    };

      context.NoteTags.AddRange(noteTags);
      await context.SaveChangesAsync();
    }
  }
}
