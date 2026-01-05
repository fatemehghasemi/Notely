using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Notely.Core.Application.Features.Categories.Queries.GetAllCategories;

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
}
