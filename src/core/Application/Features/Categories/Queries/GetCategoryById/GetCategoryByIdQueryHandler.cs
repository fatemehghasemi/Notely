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

        var response = new GetCategoryByIdResponse
        {
            Id = category.Id,
            Title = category.Title,
            NotesCount = category.Notes?.Count ?? 0,
            Notes = category.Notes?.Select(n => new CategoryNoteItem
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content.Length > 100 ? n.Content.Substring(0, 100) + "..." : n.Content,
                CreatedAt = n.CreatedAt
            }).ToList() ?? new List<CategoryNoteItem>(),
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };

        return Result<GetCategoryByIdResponse?>.Success(response);
    }
}