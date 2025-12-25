using MediatR;
using Notely.Core.Application.Interfaces.Repositories;

namespace Notely.Core.Application.Features.Notes.Queries.GetAllNotes;

public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, GetAllNotesResponse>
{
    private readonly INoteRepository _noteRepository;

    public GetAllNotesQueryHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<GetAllNotesResponse> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
    {
        var notes = await _noteRepository.GetAllWithDetailsAsync(cancellationToken);

        var noteItems = notes.Select(note => new NoteItem
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
        }).ToList();

        return new GetAllNotesResponse
        {
            Notes = noteItems
        };
    }
}
