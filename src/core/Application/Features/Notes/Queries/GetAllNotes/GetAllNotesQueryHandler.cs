using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Notes;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Queries.GetAllNotes;

public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, Result<GetAllNotesResponse>>
{
    private readonly INoteRepository _noteRepository;

    public GetAllNotesQueryHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Result<GetAllNotesResponse>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
    {
        try
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
                Tags = note.NoteTags?.Select(nt => nt.Tag.Title).ToList() ?? new List<string>(),
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt ?? note.CreatedAt
            }).ToList();

            var response = new GetAllNotesResponse
            {
                Notes = noteItems
            };

            return Result<GetAllNotesResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GetAllNotesResponse>.Failure($"Failed to get notes: {ex.Message}");
        }
    }
}
