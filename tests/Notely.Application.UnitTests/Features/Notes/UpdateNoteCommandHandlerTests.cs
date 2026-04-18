using FluentAssertions;
using Moq;
using Notely.Core.Application.Features.Notes.Commands.UpdateNote;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;

namespace Notely.Application.UnitTests.Features.Notes;

public class UpdateNoteCommandHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private UpdateNoteCommandHandler CreateHandler()
        => new(_noteRepositoryMock.Object, _tagRepositoryMock.Object);

    [Fact]
    public async Task Should_Return_Failure_When_Note_Not_Found()
    {
        var command = new UpdateNoteCommand(
            Id: Guid.NewGuid(),
            Title: "Updated title",
            Content: "Updated content",
            IsPinned: true);

        _noteRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Note?)null);

        var handler = CreateHandler();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain($"Note with ID {command.Id} not found.");

        _noteRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _tagRepositoryMock.Verify(
            x => x.GetByTitleAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Should_Update_Note_With_Existing_And_New_Tags()
    {
        var noteId = Guid.NewGuid();
        var existingWorkTag = new Tag { Id = Guid.NewGuid(), Title = "work" };
        var oldTag = new NoteTag { NoteId = noteId, TagId = Guid.NewGuid() };

        var existingNote = new Note
        {
            Id = noteId,
            Title = "Old title",
            Content = "Old content",
            IsPinned = false,
            CategoryId = Guid.NewGuid(),
            NoteTags = new List<NoteTag> { oldTag }
        };

        var command = new UpdateNoteCommand(
            Id: noteId,
            Title: "Updated title",
            Content: "Updated content",
            IsPinned: true,
            CategoryId: Guid.NewGuid(),
            Tags: ["work", "urgent"]);

        _noteRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(noteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingNote);

        _tagRepositoryMock
            .Setup(x => x.GetByTitleAsync("work", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingWorkTag);

        _tagRepositoryMock
            .Setup(x => x.GetByTitleAsync("urgent", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        _tagRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag t, CancellationToken _) => t);

        _noteRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Note n, CancellationToken _) => n);

        var handler = CreateHandler();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        _noteRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<Note>(n =>
                    n.Id == noteId &&
                    n.Title == "Updated title" &&
                    n.Content == "Updated content" &&
                    n.IsPinned &&
                    n.CategoryId == command.CategoryId &&
                    n.NoteTags.Count == 2),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _tagRepositoryMock.Verify(
            x => x.GetByTitleAsync("work", It.IsAny<CancellationToken>()),
            Times.Once);

        _tagRepositoryMock.Verify(
            x => x.GetByTitleAsync("urgent", It.IsAny<CancellationToken>()),
            Times.Once);

        _tagRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Tag>(t => t.Title == "urgent"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_Clear_Tags_When_Request_Has_No_Tags()
    {
        var noteId = Guid.NewGuid();
        var existingNote = new Note
        {
            Id = noteId,
            Title = "Old title",
            Content = "Old content",
            NoteTags = new List<NoteTag>
            {
                new() { NoteId = noteId, TagId = Guid.NewGuid() }
            }
        };

        var command = new UpdateNoteCommand(
            Id: noteId,
            Title: "Updated title",
            Content: "Updated content",
            IsPinned: false,
            Tags: null);

        _noteRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(noteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingNote);

        _noteRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Note n, CancellationToken _) => n);

        var handler = CreateHandler();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        _noteRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<Note>(n => n.NoteTags.Count == 0),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _tagRepositoryMock.Verify(
            x => x.GetByTitleAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _tagRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Should_Return_Failure_When_Exception_Is_Thrown()
    {
        var noteId = Guid.NewGuid();
        var existingNote = new Note
        {
            Id = noteId,
            Title = "Old title",
            Content = "Old content"
        };

        var command = new UpdateNoteCommand(
            Id: noteId,
            Title: "Updated title",
            Content: "Updated content",
            IsPinned: true);

        _noteRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(noteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingNote);

        _noteRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("db error"));

        var handler = CreateHandler();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Failed to update note: db error");
    }
}
