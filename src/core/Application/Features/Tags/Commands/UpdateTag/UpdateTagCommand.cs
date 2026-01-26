using MediatR;
using Notely.Core.Application.Responses.Tags;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Commands.UpdateTag;

public record UpdateTagCommand(Guid Id, string Title) : IRequest<Result<TagResponse>>;