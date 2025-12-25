using MediatR;
using Notely.Core.Application.Interfaces.Repositories;

namespace Notely.Core.Application.Features.Notes.Queries.GetNoteById;

public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, GetNoteByIdResponse?>
{
    private readonly INoteRepository _noteRepository;

    public GetNoteByIdQueryHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<GetNoteByIdResponse?> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        
        if (note == null)
            return null;

        return new GetNoteByIdResponse
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            IsPinned = note.IsPinned,
            CategoryId = note.CategoryId,
            CategoryName = note.Category?.Title,
            Tags = new List<string>(),
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt ?? note.CreatedAt
        };
    }
}
