using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Commands.DeleteTag;

public sealed class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Result>
{
    private readonly ITagRepository _tagRepository;

    public DeleteTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (tag == null)
        {
            return Result.Failure("Tag not found");
        }

        var tagWithNotes = await _tagRepository.GetByIdWithNotesAsync(request.Id, cancellationToken);
        if (tagWithNotes?.NoteTags?.Any() == true)
        {
            return Result.Failure("Cannot delete tag that is being used by notes. Please remove the tag from notes first.");
        }

        var deleted = await _tagRepository.DeleteAsync(request.Id, cancellationToken);
        
        if (!deleted)
        {
            return Result.Failure("Failed to delete tag");
        }

        return Result.Success();
    }
}
