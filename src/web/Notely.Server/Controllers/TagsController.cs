using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notely.Core.Application.Features.Tags.Commands.CreateTag;
using Notely.Core.Application.Features.Tags.Commands.DeleteTag;
using Notely.Core.Application.Features.Tags.Commands.UpdateTag;
using Notely.Core.Application.Features.Tags.Queries.GetAllTags;
using Notely.Core.Application.Features.Tags.Queries.GetTagById;
using Shared.DTOs;

namespace Notely.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
    {
        var result = await _mediator.Send(new GetAllTagsQuery());

        if (result.IsSuccess && result.Data != null)
        {
            var tagDtos = result.Data.Tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                Title = tag.Title,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            });

            return Ok(tagDtos);
        }

        return BadRequest(result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(Guid id)
    {
        var result = await _mediator.Send(new GetTagByIdQuery(id));

        if (result.IsSuccess && result.Data != null)
        {
            var tagDto = new TagDto
            {
                Id = result.Data.Id,
                Title = result.Data.Title,
                NotesCount = result.Data.NotesCount,
                CreatedAt = result.Data.CreatedAt,
                UpdatedAt = result.Data.UpdatedAt
            };

            return Ok(tagDto);
        }

        var errors = result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" };
        if (errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase)))
        {
            return NotFound(errors);
        }

        return BadRequest(errors);
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag([FromBody] CreateTagDto createTagDto)
    {
        var command = new CreateTagCommand(createTagDto.Title);
        var result = await _mediator.Send(command);

        if (result.IsSuccess && result.Data != null)
        {
            var getResult = await _mediator.Send(new GetTagByIdQuery(result.Data.Id));
            if (getResult.IsSuccess && getResult.Data != null)
            {
                var tagDto = new TagDto
                {
                    Id = getResult.Data.Id,
                    Title = getResult.Data.Title,
                    NotesCount = getResult.Data.NotesCount,
                    CreatedAt = getResult.Data.CreatedAt,
                    UpdatedAt = getResult.Data.UpdatedAt
                };

                return CreatedAtAction(nameof(GetTag), new { id = tagDto.Id }, tagDto);
            }
        }

        return BadRequest(result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TagDto>> UpdateTag(Guid id, [FromBody] UpdateTagDto updateTagDto)
    {
        var command = new UpdateTagCommand(id, updateTagDto.Title);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var getResult = await _mediator.Send(new GetTagByIdQuery(id));
            if (getResult.IsSuccess && getResult.Data != null)
            {
                var tagDto = new TagDto
                {
                    Id = getResult.Data.Id,
                    Title = getResult.Data.Title,
                    NotesCount = getResult.Data.NotesCount,
                    CreatedAt = getResult.Data.CreatedAt,
                    UpdatedAt = getResult.Data.UpdatedAt
                };

                return Ok(tagDto);
            }
        }

        var errors = result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" };
        if (errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase)))
        {
            return NotFound(errors);
        }

        return BadRequest(errors);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag(Guid id)
    {
        var result = await _mediator.Send(new DeleteTagCommand(id));

        if (result.IsSuccess)
        {
            return NoContent();
        }

        var errors = result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" };
        if (errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase)))
        {
            return NotFound(errors);
        }

        return BadRequest(errors);
    }
}
