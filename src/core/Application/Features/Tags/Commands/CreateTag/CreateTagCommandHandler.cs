using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Tags;
using Notely.Core.Domain.Entities;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Commands.CreateTag;

public sealed class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<CreateTagResponse>>
{
    private readonly ITagRepository _tagRepository;

    public CreateTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<CreateTagResponse>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var existingTag = await _tagRepository.GetByTitleAsync(request.Title, cancellationToken);
        if (existingTag != null)
        {
            return Result<CreateTagResponse>.Failure("A tag with this title already exists");
        }

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            CreatedAt = DateTime.UtcNow
        };

        var createdTag = await _tagRepository.AddAsync(tag, cancellationToken);
        var response = createdTag.Adapt<CreateTagResponse>();

        return Result<CreateTagResponse>.Success(response);
    }
}
