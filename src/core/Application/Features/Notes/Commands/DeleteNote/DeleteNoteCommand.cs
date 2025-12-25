using MediatR;

namespace Notely.Core.Application.Features.Notes.Commands.DeleteNote;

public record DeleteNoteCommand(Guid Id) : IRequest<bool>;