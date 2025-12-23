using Microsoft.EntityFrameworkCore;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;
using Notely.Infrastructure.Persistence;

namespace Notely.Infrastructure.Repositories;

public sealed class NoteRepository : BaseRepository<Note, Guid>, INoteRepository
{
  public NoteRepository(BlazorNotelyContext context) : base(context)
  {
  }
  public async Task<Note?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
  {
    return await _dbSet
        .Include(n => n.Category)
        .Include(n => n.NoteTags)
            .ThenInclude(nt => nt.Tag)
        .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
  }

  public async Task<IReadOnlyList<Note>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
  {
    return await _dbSet
        .Include(n => n.Category)
        .Include(n => n.NoteTags)
            .ThenInclude(nt => nt.Tag)
        .OrderByDescending(n => n.CreatedAt)
        .ToListAsync(cancellationToken);
  }

  public async Task<IReadOnlyList<Note>> SearchNotesAsync(string searchTerm, CancellationToken cancellationToken = default)
  {
    return await _dbSet
        .Include(n => n.Category)
        .Include(n => n.NoteTags)
            .ThenInclude(nt => nt.Tag)
        .Where(n => n.Title.Contains(searchTerm) || n.Content.Contains(searchTerm))
        .OrderByDescending(n => n.CreatedAt)
        .ToListAsync(cancellationToken);
  }
}