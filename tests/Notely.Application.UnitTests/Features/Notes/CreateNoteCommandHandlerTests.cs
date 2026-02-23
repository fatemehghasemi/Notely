using FluentAssertions;
using Moq;
using Notely.Core.Application.Features.Notes.Commands.CreateNote;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;

namespace Notely.Application.UnitTests.Features.Notes;

public class CreateNoteCommandHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private CreateNoteCommandHandler CreateHandler()
        => new(_noteRepositoryMock.Object, _tagRepositoryMock.Object);

    [Fact]
    public async Task Should_Create_Note_With_New_And_Existing_Tags()
    {
        var command = new CreateNoteCommand(
            Title: "  Title  ",
            Content: "  Content  ",
            CategoryId: Guid.NewGuid(),
            Tags: ["work", "Work", "planning"]);

        var existingTag = new Tag
        {
            Id = Guid.NewGuid(),
            Title = "work"
        };

        _tagRepositoryMock
            .Setup(x => x.GetByTitlesAsync(
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingTag]);

        _tagRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag t, CancellationToken _) => t);

        _noteRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Note n, CancellationToken _) => n);

        var handler = CreateHandler();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        _tagRepositoryMock.Verify(
            x => x.GetByTitlesAsync(
                It.Is<IEnumerable<string>>(t =>
                    t.Count() == 2 &&
                    t.Contains("work") &&
                    t.Contains("planning")),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _tagRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Tag>(t => t.Title == "planning"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _noteRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Note>(n =>
                    n.Title == "Title" &&
                    n.Content == "Content" &&
                    n.NoteTags.Count == 2),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
