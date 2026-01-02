using MediatR;
using Notely.Core.Application.Features.Tags.Queries.GetAllTags;
using Shared.DTOs;

namespace Notely.Web.Services.AppServices;

public class TagsAppService : ITagsAppService
{
    private readonly IMediator _mediator;

    public TagsAppService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllTagsQuery(), cancellationToken);

        if (!result.IsSuccess || result.Data?.Tags == null)
            return Enumerable.Empty<TagDto>();

        return result.Data.Tags.Select(tag => new TagDto
        {
            Id = tag.Id,
            Title = tag.Title
        });
    }

}
