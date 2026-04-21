using FluentAssertions;
using Moq;
using Notely.Core.Application.Features.Notes.Queries.GetAllNotes;
using Notely.Core.Application.Features.Notes.Queries.GetNoteById;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;

namespace Notely.Application.UnitTests.Features.Notes;

public class NoteQueryHandlersTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock = new();

    private GetAllNotesQueryHandler CreateGetAllHandler()
        => new(_noteRepositoryMock.Object);

    private GetNoteByIdQueryHandler CreateGetByIdHandler()
        => new(_noteRepositoryMock.Object);

    [Fact]
    public async Task GetAllNotes_Should_Return_Success_When_Notes_Exist()
    {
        var notes = new List<Note>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "First",
                Content = "First content",
                IsPinned = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Second",
                Content = "Second content",
                IsPinned = false
            }
        };

        _noteRepositoryMock
            .Setup(x => x.GetAllWithDetailsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(notes);

        var handler = CreateGetAllHandler();

        var result = await handler.Handle(new GetAllNotesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Notes.Should().HaveCount(2);
        result.Data.Notes.Select(n => n.Title).Should().Contain(["First", "Second"]);

        _noteRepositoryMock.Verify(
            x => x.GetAllWithDetailsAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllNotes_Should_Return_Failure_When_Repository_Throws()
    {
        _noteRepositoryMock
            .Setup(x => x.GetAllWithDetailsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("db error"));

        var handler = CreateGetAllHandler();

        var result = await handler.Handle(new GetAllNotesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Failed to get notes: db error");
    }

    [Fact]
    public async Task GetNoteById_Should_Return_Success_Null_When_Note_Not_Found()
    {
        var id = Guid.NewGuid();

        _noteRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Note?)null);

        var handler = CreateGetByIdHandler();

        var result = await handler.Handle(new GetNoteByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetNoteById_Should_Return_Success_Response_When_Note_Exists()
    {
        var id = Guid.NewGuid();
        var note = new Note
        {
            Id = id,
            Title = "Title",
            Content = "Content",
            IsPinned = true
        };

        _noteRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(note);

        var handler = CreateGetByIdHandler();

        var result = await handler.Handle(new GetNoteByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(id);
        result.Data.Title.Should().Be("Title");
    }

    [Fact]
    public async Task GetNoteById_Should_Return_Failure_When_Repository_Throws()
    {
        var id = Guid.NewGuid();

        _noteRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("db error"));

        var handler = CreateGetByIdHandler();

        var result = await handler.Handle(new GetNoteByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Failed to get note: db error");
    }
}
