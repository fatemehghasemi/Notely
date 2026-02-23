using Notely.Core.Domain.Entities;

namespace Notely.Core.Application.Interfaces.Repositories;

public interface ITagRepository : IBaseRepository<Tag, Guid>
{
    Task<IReadOnlyList<Tag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<Tag?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<Tag?> GetByIdWithNotesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Tag>> GetByTitlesAsync(
    IEnumerable<string> titles,
    CancellationToken cancellationToken = default);
}
