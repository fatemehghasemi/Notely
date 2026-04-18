using FluentAssertions;
using Notely.Application.UnitTests.Common;
using Notely.Core.Application.Features.Notes.Commands.CreateNote;
using Notely.Core.Application.Features.Notes.Commands.UpdateNote;

namespace Notely.Application.UnitTests.Features.Notes;

public class NoteCommandValidatorsTests
{
    [Fact]
    public void CreateNoteValidator_Should_Pass_For_Valid_Command()
    {
        var validator = new CreateNoteCommandValidator(new TestStringLocalizer<CreateNoteCommandValidator>());
        var command = new CreateNoteCommand(
            Title: "Meeting notes",
            Content: "Discuss Q1 roadmap",
            CategoryId: Guid.NewGuid(),
            Tags: ["work", "planning"]);

        var result = validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateNoteValidator_Should_Fail_When_Title_Is_Empty()
    {
        var validator = new CreateNoteCommandValidator(new TestStringLocalizer<CreateNoteCommandValidator>());
        var command = new CreateNoteCommand(
            Title: string.Empty,
            Content: "Content is present",
            CategoryId: Guid.NewGuid());

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void CreateNoteValidator_Should_Fail_When_Any_Tag_Is_Whitespace()
    {
        var validator = new CreateNoteCommandValidator(new TestStringLocalizer<CreateNoteCommandValidator>());
        var command = new CreateNoteCommand(
            Title: "Valid title",
            Content: "Valid content",
            CategoryId: Guid.NewGuid(),
            Tags: ["valid-tag", " "]);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Tags"));
    }

    [Fact]
    public void CreateNoteValidator_Should_Fail_When_Title_Exceeds_Max_Length()
    {
        var validator = new CreateNoteCommandValidator(new TestStringLocalizer<CreateNoteCommandValidator>());
        var command = new CreateNoteCommand(
            Title: new string('a', 201),
            Content: "Valid content",
            CategoryId: Guid.NewGuid());

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void CreateNoteValidator_Should_Fail_When_Title_Or_Content_Is_Whitespace()
    {
        var validator = new CreateNoteCommandValidator(new TestStringLocalizer<CreateNoteCommandValidator>());
        var command = new CreateNoteCommand(
            Title: " ",
            Content: " ",
            CategoryId: Guid.NewGuid());

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
        result.Errors.Should().Contain(e => e.PropertyName == "Content");
    }

    [Fact]
    public void CreateNoteValidator_Should_Fail_When_Tags_Are_Duplicated_Ignoring_Case()
    {
        var validator = new CreateNoteCommandValidator(new TestStringLocalizer<CreateNoteCommandValidator>());
        var command = new CreateNoteCommand(
            Title: "Valid title",
            Content: "Valid content",
            CategoryId: Guid.NewGuid(),
            Tags: ["Work", "work"]);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Tags");
    }

    [Fact]
    public void UpdateNoteValidator_Should_Fail_When_Id_Is_Empty()
    {
        var validator = new UpdateNoteCommandValidator(new TestStringLocalizer<UpdateNoteCommandValidator>());
        var command = new UpdateNoteCommand(
            Id: Guid.Empty,
            Title: "Valid title",
            Content: "Valid content",
            IsPinned: false);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void UpdateNoteValidator_Should_Fail_When_Title_Exceeds_Max_Length()
    {
        var validator = new UpdateNoteCommandValidator(new TestStringLocalizer<UpdateNoteCommandValidator>());
        var command = new UpdateNoteCommand(
            Id: Guid.NewGuid(),
            Title: new string('a', 201),
            Content: "Valid content",
            IsPinned: false);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void UpdateNoteValidator_Should_Fail_When_CategoryId_Is_Empty_Guid()
    {
        var validator = new UpdateNoteCommandValidator(new TestStringLocalizer<UpdateNoteCommandValidator>());
        var command = new UpdateNoteCommand(
            Id: Guid.NewGuid(),
            Title: "Valid title",
            Content: "Valid content",
            IsPinned: false,
            CategoryId: Guid.Empty);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
    }

    [Fact]
    public void UpdateNoteValidator_Should_Fail_When_Any_Tag_Is_Whitespace()
    {
        var validator = new UpdateNoteCommandValidator(new TestStringLocalizer<UpdateNoteCommandValidator>());
        var command = new UpdateNoteCommand(
            Id: Guid.NewGuid(),
            Title: "Valid title",
            Content: "Valid content",
            IsPinned: false,
            Tags: ["ok", " "]);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Tags");
    }
}
