using Microsoft.EntityFrameworkCore;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;
using Notely.Infrastructure.Persistence;

namespace Notely.Infrastructure.Repositories;

public sealed class TagRepository : BaseRepository<Tag, Guid>, ITagRepository
{
    public TagRepository(BlazorNotelyContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Tag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Tag?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(t => t.Title == title, cancellationToken);
    }

    public async Task<Tag?> GetByIdWithNotesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.NoteTags)
            .ThenInclude(nt => nt.Note)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
