using Shared.DTOs;

namespace Notely.Client.Services.Tags;

public interface ITagsService
{
    Task<IEnumerable<TagDto>> GetTagsAsync();
    Task<TagDto?> GetTagAsync(Guid id);
    Task<TagDto> CreateTagAsync(CreateTagDto createTagDto);
    Task<TagDto> UpdateTagAsync(Guid id, UpdateTagDto updateTagDto);
    Task DeleteTagAsync(Guid id);
}
