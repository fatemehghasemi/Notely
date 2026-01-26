using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Categories;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (category == null)
        {
            return Result<CategoryResponse>.Failure("Category not found");
        }

        category.Title = request.Title;

        var updatedCategory = await _categoryRepository.UpdateAsync(category, cancellationToken);
        var response = updatedCategory.Adapt<CategoryResponse>();

        return Result<CategoryResponse>.Success(response);
    }
}
