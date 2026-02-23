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
            Content: "Content is present");

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
            Tags: ["valid-tag", " "]);

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
}
