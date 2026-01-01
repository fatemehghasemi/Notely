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
    private readonly ITagRepository _tagRepository;

    public CreateNoteCommandHandler(INoteRepository noteRepository, ITagRepository tagRepository)
    {
        _noteRepository = noteRepository;
        _tagRepository = tagRepository;
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
            };

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
