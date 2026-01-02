using MediatR;
using Notely.Core.Application.Responses.Tags;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Queries.GetAllTags;

public record GetAllTagsQuery() : IRequest<Result<GetAllTagsResponse>>;