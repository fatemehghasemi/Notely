using FluentAssertions;
using Moq;
using Notely.Core.Application.Features.Tags.Commands.CreateTag;
using Notely.Core.Application.Features.Tags.Commands.DeleteTag;
using Notely.Core.Application.Features.Tags.Commands.UpdateTag;
using Notely.Core.Application.Features.Tags.Queries.GetAllTags;
using Notely.Core.Application.Features.Tags.Queries.GetTagById;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;

namespace Notely.Application.UnitTests.Features.Tags;

public class TagHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    [Fact]
    public async Task CreateTag_Should_Return_Failure_When_Title_Already_Exists()
    {
        var command = new CreateTagCommand("work");

        _tagRepositoryMock
            .Setup(x => x.GetByTitleAsync(command.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Tag { Id = Guid.NewGuid(), Title = "work" });

        var handler = new CreateTagCommandHandler(_tagRepositoryMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("A tag with this title already exists");

        _tagRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateTag_Should_Return_Success_When_Title_Is_New()
    {
        var command = new CreateTagCommand("work");

        _tagRepositoryMock
            .Setup(x => x.GetByTitleAsync(command.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        _tagRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag t, CancellationToken _) => t);

        var handler = new CreateTagCommandHandler(_tagRepositoryMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Title.Should().Be("work");
    }

    [Fact]
    public async Task UpdateTag_Should_Return_Failure_When_Tag_Not_Found()
    {
        var command = new UpdateTagCommand(Guid.NewGuid(), "updated");

        _tagRepositoryMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        var handler = new UpdateTagCommandHandler(_tagRepositoryMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Tag not found");
    }

    [Fact]
    public async Task DeleteTag_Should_Return_Failure_When_Tag_Is_Used_By_Notes()
    {
        var id = Guid.NewGuid();
        var command = new DeleteTagCommand(id);

        _tagRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Tag { Id = id, Title = "work" });

        _tagRepositoryMock
            .Setup(x => x.GetByIdWithNotesAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Tag
            {
                Id = id,
                Title = "work",
                NoteTags = new List<NoteTag>
                {
                    new() { NoteId = Guid.NewGuid(), TagId = id }
                }
            });

        var handler = new DeleteTagCommandHandler(_tagRepositoryMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Cannot delete tag that is being used by notes");
    }

    [Fact]
    public async Task GetAllTags_Should_Return_Success_With_Items()
    {
        var tags = new List<Tag>
        {
            new() { Id = Guid.NewGuid(), Title = "work", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "personal", CreatedAt = DateTime.UtcNow }
        };

        _tagRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        var handler = new GetAllTagsQueryHandler(_tagRepositoryMock.Object);

        var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Tags.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllTags_Should_Return_Failure_When_Repository_Throws()
    {
        _tagRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("db error"));

        var handler = new GetAllTagsQueryHandler(_tagRepositoryMock.Object);

        var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Failed to get tags: db error");
    }

    [Fact]
    public async Task GetTagById_Should_Return_Failure_When_Not_Found()
    {
        var id = Guid.NewGuid();

        _tagRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        var handler = new GetTagByIdQueryHandler(_tagRepositoryMock.Object);

        var result = await handler.Handle(new GetTagByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Tag not found");
    }
}
