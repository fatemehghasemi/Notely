using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Categories;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryByIdResponse?>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<GetCategoryByIdResponse?>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdWithNotesAsync(request.Id, cancellationToken);
        
        if (category == null)
        {
            return Result<GetCategoryByIdResponse?>.Failure("Category not found");
        }

        var response = category.Adapt<GetCategoryByIdResponse>();

        return Result<GetCategoryByIdResponse?>.Success(response);
    }
}