using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Tags;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Queries.GetTagById;

public sealed class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Result<GetTagByIdResponse?>>
{
    private readonly ITagRepository _tagRepository;

    public GetTagByIdQueryHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<GetTagByIdResponse?>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (tag == null)
        {
            return Result<GetTagByIdResponse?>.Failure("Tag not found");
        }

        var response = tag.Adapt<GetTagByIdResponse>();

        return Result<GetTagByIdResponse?>.Success(response);
    }
}