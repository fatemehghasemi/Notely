using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Tags;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Commands.UpdateTag;

public sealed class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Result<UpdateTagResponse>>
{
    private readonly ITagRepository _tagRepository;

    public UpdateTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<UpdateTagResponse>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id, cancellationToken);

        if (tag == null)
        {
            return Result<UpdateTagResponse>.Failure("Tag not found");
        }

        var existingTag = await _tagRepository.GetByTitleAsync(request.Title, cancellationToken);
        if (existingTag != null && existingTag.Id != request.Id)
        {
            return Result<UpdateTagResponse>.Failure("A tag with this title already exists");
        }

        tag.Title = request.Title;
        tag.UpdatedAt = DateTime.UtcNow;

        var updatedTag = await _tagRepository.UpdateAsync(tag, cancellationToken);
        var response = updatedTag.Adapt<UpdateTagResponse>();

        return Result<UpdateTagResponse>.Success(response);
    }
}
