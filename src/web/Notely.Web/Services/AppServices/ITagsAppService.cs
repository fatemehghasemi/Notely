using Shared.DTOs;

namespace Notely.Web.Services.AppServices;

public interface ITagsAppService
{
    Task<IEnumerable<TagDto>> GetAllTagsAsync(CancellationToken cancellationToken = default);
}
