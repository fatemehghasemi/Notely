using Notely.Core.Domain.Entities;

namespace Notely.Core.Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category, Guid>
{
    Task<Category?> GetByIdWithNotesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> GetAllWithNotesCountAsync(CancellationToken cancellationToken = default);
}
