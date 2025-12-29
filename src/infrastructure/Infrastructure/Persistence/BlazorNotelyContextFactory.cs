using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notely.Infrastructure.Persistence;

public class BlazorNotelyContextFactory : IDesignTimeDbContextFactory<BlazorNotelyContext>
{
    public BlazorNotelyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlazorNotelyContext>();
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=NotelyDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new BlazorNotelyContext(optionsBuilder.Options);
    }
}