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

            var noteItems = notes.Adapt<List<NoteItem>>();

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
