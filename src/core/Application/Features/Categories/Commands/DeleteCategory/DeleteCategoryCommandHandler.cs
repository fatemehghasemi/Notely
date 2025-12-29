using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (category == null)
        {
            return Result.Failure("Category not found");
        }

        var categoryWithNotes = await _categoryRepository.GetByIdWithNotesAsync(request.Id, cancellationToken);
        if (categoryWithNotes?.Notes?.Any() == true)
        {
            return Result.Failure("Cannot delete category that contains notes. Please move or delete the notes first.");
        }

        var deleted = await _categoryRepository.DeleteAsync(request.Id, cancellationToken);
        
        if (!deleted)
        {
            return Result.Failure("Failed to delete category");
        }

        return Result.Success();
    }
}
