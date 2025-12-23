using Notely.Core.Domain.Entities;

namespace Notely.Core.Application.Interfaces.Repositories;

public interface INoteRepository : IBaseRepository<Note, Guid>
{
  Task<Note?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
  Task<IReadOnlyList<Note>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyList<Note>> SearchNotesAsync(string searchTerm, CancellationToken cancellationToken = default);
}