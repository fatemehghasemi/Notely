using FluentAssertions;
using Moq;
using Notely.Core.Application.Features.Categories.Commands.CreateCategory;
using Notely.Core.Application.Features.Categories.Commands.DeleteCategory;
using Notely.Core.Application.Features.Categories.Commands.UpdateCategory;
using Notely.Core.Application.Features.Categories.Queries.GetAllCategories;
using Notely.Core.Application.Features.Categories.Queries.GetCategoryById;
using Notely.Core.Application.Interfaces.Repositories;
using Notely.Core.Domain.Entities;

namespace Notely.Application.UnitTests.Features.Categories;

public class CategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();

    [Fact]
    public async Task CreateCategory_Should_Return_Success()
    {
        var command = new CreateCategoryCommand("Work");

        _categoryRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category c, CancellationToken _) => c);

        var handler = new CreateCategoryCommandHandler(_categoryRepositoryMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Title.Should().Be("Work");
    }

    [Fact]
    public async Task UpdateCategory_Should_Return_Failure_When_Not_Found()
    {
        var command = new UpdateCategoryCommand(Guid.NewGuid(), "Updated");

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var handler = new UpdateCategoryCommandHandler(_categoryRepositoryMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Category not found");
    }

    [Fact]
    public async Task DeleteCategory_Should_Return_Failure_When_Contains_Notes()
    {
        var id = Guid.NewGuid();
        var command = new DeleteCategoryCommand(id);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = id, Title = "Work" });

        _categoryRepositoryMock
            .Setup(x => x.GetByIdWithNotesAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category
            {
                Id = id,
                Title = "Work",
                Notes = new List<Note>
                {
                    new() { Id = Guid.NewGuid(), Title = "N1", Content = "C1" }
                }
            });

        var handler = new DeleteCategoryCommandHandler(_categoryRepositoryMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Cannot delete category that contains notes");
    }

    [Fact]
    public async Task GetAllCategories_Should_Return_Success_With_Items()
    {
        var categories = new List<Category>
        {
            new() { Id = Guid.NewGuid(), Title = "Work" },
            new() { Id = Guid.NewGuid(), Title = "Personal" }
        };

        _categoryRepositoryMock
            .Setup(x => x.GetAllWithNotesCountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        var handler = new GetAllCategoriesQueryHandler(_categoryRepositoryMock.Object);

        var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Categories.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCategoryById_Should_Return_Failure_When_Not_Found()
    {
        var id = Guid.NewGuid();

        _categoryRepositoryMock
            .Setup(x => x.GetByIdWithNotesAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var handler = new GetCategoryByIdQueryHandler(_categoryRepositoryMock.Object);

        var result = await handler.Handle(new GetCategoryByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Category not found");
    }
}
