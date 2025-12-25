using MediatR;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Commands.DeleteNote;

public record DeleteNoteCommand(Guid Id) : IRequest<Result>;
