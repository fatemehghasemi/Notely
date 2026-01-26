using MediatR;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Tags.Commands.DeleteTag;

public record DeleteTagCommand(Guid Id) : IRequest<Result>;