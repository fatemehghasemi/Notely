using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notely.Core.Application.Features.Notes.Commands.CreateNote;
using Notely.Core.Application.Features.Notes.Queries.GetAllNotes;
using Notely.Core.Application.Features.Notes.Queries.GetNoteById;

namespace BlazorNotely.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotes()
    {
        var result = await _mediator.Send(new GetAllNotesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        var result = await _mediator.Send(new GetNoteByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetNoteById), new { id = result.Data?.Id }, result);
    }
}