using Microsoft.EntityFrameworkCore;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;
using Notely.Infrastructure.Persistence;

namespace Notely.Infrastructure.Repositories;

public sealed class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(BlazorNotelyContext context) : base(context)
    {
    }

    public async Task<Category?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Title == title, cancellationToken);
    }
}