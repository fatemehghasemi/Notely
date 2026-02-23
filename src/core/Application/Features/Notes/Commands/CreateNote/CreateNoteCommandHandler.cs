using Mapster;
using MediatR;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Application.Responses.Notes;
using Notely.Core.Domain.Entities;
using Shared.Wrapper;

namespace Notely.Core.Application.Features.Notes.Commands.CreateNote;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Result<NoteResponse>>
{
    private readonly INoteRepository _noteRepository;
    private readonly ITagRepository _tagRepository;

    public CreateNoteCommandHandler(INoteRepository noteRepository, ITagRepository tagRepository)
    {
        _noteRepository = noteRepository;
        _tagRepository = tagRepository;
    }

    public async Task<Result<NoteResponse>> Handle(
        CreateNoteCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                IsPinned = request.IsPinned,
                CategoryId = request.CategoryId
            };

            if (request.Tags?.Any() == true)
            {
                var normalizedTags = request.Tags
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var existingTags = (await _tagRepository
                    .GetByTitlesAsync(normalizedTags, cancellationToken))
                    .ToList();

                var existingTitles = existingTags
                    .Select(t => t.Title)
                    .ToList();

                var missingTitles = normalizedTags
                    .Except(existingTitles, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                foreach (var title in missingTitles)
                {
                    var newTag = await _tagRepository.AddAsync(
                        new Tag
                        {
                            Id = Guid.NewGuid(),
                            Title = title
                        },
                        cancellationToken);

                    existingTags.Add(newTag);
                }

                note.NoteTags = existingTags
                    .Select(tag => new NoteTag
                    {
                        NoteId = note.Id,
                        TagId = tag.Id
                    })
                    .ToList();
            }

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);

            return Result<NoteResponse>.Success(createdNote.Adapt<NoteResponse>());
        }
        catch (Exception ex)
        {
            return Result<NoteResponse>.Failure($"Failed to create note: {ex.Message}");
        }
    }
}
