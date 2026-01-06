using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Notes;
using Shared.Wrapper;
using Mapster;

namespace Notely.Core.Application.Features.Notes.Queries.GetNoteById;

public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, Result<GetNoteByIdResponse?>>
{
    private readonly INoteRepository _noteRepository;

    public GetNoteByIdQueryHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Result<GetNoteByIdResponse?>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var note = await _noteRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            
            if (note == null)
            {
                return Result<GetNoteByIdResponse?>.Success(null);
            }

            var response = note.Adapt<GetNoteByIdResponse>();

            return Result<GetNoteByIdResponse?>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GetNoteByIdResponse?>.Failure($"Failed to get note: {ex.Message}");
        }
    }
}
