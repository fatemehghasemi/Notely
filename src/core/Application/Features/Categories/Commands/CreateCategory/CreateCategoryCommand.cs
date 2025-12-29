using MediatR;
using Notely.Core.Application.Responses.Categories;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Title) : IRequest<Result<CreateCategoryResponse>>;