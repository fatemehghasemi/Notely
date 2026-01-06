using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Categories;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<GetAllCategoriesResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<GetAllCategoriesResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllWithNotesCountAsync(cancellationToken);
        
        var categoryItems = categories.Adapt<List<CategoryItem>>();

        var response = new GetAllCategoriesResponse
        {
            Categories = categoryItems
        };

        return Result<GetAllCategoriesResponse>.Success(response);
    }
}