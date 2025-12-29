using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Notes;
using Notely.Core.Domain.Entities;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Commands.CreateNote;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Result<CreateNoteResponse>>
{
    private readonly INoteRepository _noteRepository;

    public CreateNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Result<CreateNoteResponse>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                IsPinned = request.IsPinned,
                CategoryId = request.CategoryId
                // CreatedAt خودکار در SaveChangesAsync تنظیم می‌شود
                // UpdatedAt = null (default) چون هنوز update نشده
            };

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);

            var response = createdNote.Adapt<CreateNoteResponse>();

            return Result<CreateNoteResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<CreateNoteResponse>.Failure($"Failed to create note: {ex.Message}");
        }
    }
}
