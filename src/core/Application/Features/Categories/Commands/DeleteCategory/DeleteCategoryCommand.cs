using MediatR;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result>;