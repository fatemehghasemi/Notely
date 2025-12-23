using Notely.Core.Domain.Entities;

namespace Notely.Core.Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category, Guid>
{
    Task<Category?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
}