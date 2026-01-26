using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Notes;
using Notely.Core.Domain.Entities;
using Shared.Wrapper;
using Mapster;

namespace Notely.Core.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Result<NoteResponse>>
{
    private readonly INoteRepository _noteRepository;
    private readonly ITagRepository _tagRepository;

    public UpdateNoteCommandHandler(INoteRepository noteRepository, ITagRepository tagRepository)
    {
        _noteRepository = noteRepository;
        _tagRepository = tagRepository;
    }

    public async Task<Result<NoteResponse>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var note = await _noteRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            if (note == null)
            {
                return Result<NoteResponse>.Failure($"Note with ID {request.Id} not found.");
            }

            note.Title = request.Title;
            note.Content = request.Content;
            note.IsPinned = request.IsPinned;
            note.CategoryId = request.CategoryId;

            note.NoteTags.Clear();

            if (request.Tags?.Any() == true)
            {
                var noteTags = new List<NoteTag>();

                foreach (var tagTitle in request.Tags)
                {
                    var existingTag = await _tagRepository.GetByTitleAsync(tagTitle, cancellationToken);

                    if (existingTag == null)
                    {
                        existingTag = new Tag
                        {
                            Id = Guid.NewGuid(),
                            Title = tagTitle
                        };
                        existingTag = await _tagRepository.AddAsync(existingTag, cancellationToken);
                    }

                    noteTags.Add(new NoteTag
                    {
                        NoteId = note.Id,
                        TagId = existingTag.Id
                    });
                }

                note.NoteTags = noteTags;
            }

            var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);

            var response = updatedNote.Adapt<NoteResponse>();

            return Result<NoteResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<NoteResponse>.Failure($"Failed to update note: {ex.Message}");
        }
    }
}
