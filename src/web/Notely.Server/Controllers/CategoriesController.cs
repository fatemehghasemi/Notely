using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Notely.Core.Application.Features.Categories.Queries.GetAllCategories;
using Notely.Core.Application.Features.Categories.Queries.GetCategoryById;
using Notely.Core.Application.Features.Categories.Commands.CreateCategory;
using Notely.Core.Application.Features.Categories.Commands.UpdateCategory;
using Notely.Core.Application.Features.Categories.Commands.DeleteCategory;

namespace Notely.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        
        if (result.IsSuccess && result.Data != null)
        {
            var categoryDtos = result.Data.Categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Title = category.Title,
                NotesCount = category.NotesCount,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            });
            
            return Ok(categoryDtos);
        }
        
        return BadRequest(result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));

        if (result.IsSuccess && result.Data != null)
        {
            var categoryDto = new CategoryDto
            {
                Id = result.Data.Id,
                Title = result.Data.Title,
                NotesCount = result.Data.NotesCount,
                CreatedAt = result.Data.CreatedAt,
                UpdatedAt = result.Data.UpdatedAt
            };

            return Ok(categoryDto);
        }

        var errors = result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" };
        if (errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase)))
        {
            return NotFound(errors);
        }

        return BadRequest(errors);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        var command = new CreateCategoryCommand(createCategoryDto.Title);
        var result = await _mediator.Send(command);

        if (result.IsSuccess && result.Data != null)
        {
            var getResult = await _mediator.Send(new GetCategoryByIdQuery(result.Data.Id));
            if (getResult.IsSuccess && getResult.Data != null)
            {
                var categoryDto = new CategoryDto
                {
                    Id = getResult.Data.Id,
                    Title = getResult.Data.Title,
                    NotesCount = getResult.Data.NotesCount,
                    CreatedAt = getResult.Data.CreatedAt,
                    UpdatedAt = getResult.Data.UpdatedAt
                };

                return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
            }
        }

        return BadRequest(result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto updateCategoryDto)
    {
        var command = new UpdateCategoryCommand(id, updateCategoryDto.Title);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var getResult = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (getResult.IsSuccess && getResult.Data != null)
            {
                var categoryDto = new CategoryDto
                {
                    Id = getResult.Data.Id,
                    Title = getResult.Data.Title,
                    NotesCount = getResult.Data.NotesCount,
                    CreatedAt = getResult.Data.CreatedAt,
                    UpdatedAt = getResult.Data.UpdatedAt
                };

                return Ok(categoryDto);
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
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));

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
