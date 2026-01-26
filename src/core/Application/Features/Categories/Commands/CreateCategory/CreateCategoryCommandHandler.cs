using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Categories;
using Notely.Core.Domain.Entities;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Title = request.Title
        };

        var createdCategory = await _categoryRepository.AddAsync(category, cancellationToken);
        var response = createdCategory.Adapt<CategoryResponse>();

        return Result<CategoryResponse>.Success(response);
    }
}
