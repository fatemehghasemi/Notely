using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Tags;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Queries.GetAllTags;

public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, Result<GetAllTagsResponse>>
{
    private readonly ITagRepository _tagRepository;

    public GetAllTagsQueryHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<GetAllTagsResponse>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tags = await _tagRepository.GetAllAsync(cancellationToken);
            
            var response = new GetAllTagsResponse
            {
                Tags = tags.Select(tag => new TagItem
                {
                    Id = tag.Id,
                    Title = tag.Title,
                    CreatedAt = tag.CreatedAt,
                    UpdatedAt = tag.UpdatedAt
                }).ToList()
            };

            return Result<GetAllTagsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GetAllTagsResponse>.Failure($"Failed to get tags: {ex.Message}");
        }
    }
}