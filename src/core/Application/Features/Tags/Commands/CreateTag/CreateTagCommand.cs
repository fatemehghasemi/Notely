using MediatR;
using Notely.Core.Application.Responses.Tags;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Commands.CreateTag;

public record CreateTagCommand(string Title) : IRequest<Result<CreateTagResponse>>;