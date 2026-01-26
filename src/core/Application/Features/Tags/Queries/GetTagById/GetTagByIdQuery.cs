using MediatR;
using Notely.Core.Application.Responses.Tags;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Queries.GetTagById;

public record GetTagByIdQuery(Guid Id) : IRequest<Result<GetTagByIdResponse?>>;