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

    public async Task<Category?> GetByIdWithNotesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Notes)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetAllWithNotesCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Notes)
            .OrderBy(c => c.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(c => c.Title == title, cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(string title, Guid excludeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(c => c.Title == title && c.Id != excludeId, cancellationToken);
    }

    public async Task<Category?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Title == title, cancellationToken);
    }
}