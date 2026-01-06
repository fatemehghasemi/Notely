using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Notely.Core.Application.Features.Notes.Commands.CreateNote;
using Notely.Core.Application.Features.Notes.Commands.UpdateNote;
using Notely.Core.Application.Features.Notes.Commands.DeleteNote;
using Notely.Core.Application.Features.Notes.Queries.GetAllNotes;
using Notely.Core.Application.Features.Notes.Queries.GetNoteById;
using Mapster;

namespace Notely.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes()
    {
        var result = await _mediator.Send(new GetAllNotesQuery());
        
        if (result.IsSuccess && result.Data != null)
        {
            var noteDtos = result.Data.Notes.Select(note => new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                IsPinned = note.IsPinned,
                CategoryId = note.CategoryId,
                CategoryName = note.CategoryName,
                Tags = note.Tags,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            });
            
            return Ok(noteDtos);
        }
        
        return BadRequest(result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NoteDto>> GetNote(Guid id)
    {
        var result = await _mediator.Send(new GetNoteByIdQuery(id));
        
        if (result.IsSuccess && result.Data != null)
        {
            var noteDto = new NoteDto
            {
                Id = result.Data.Id,
                Title = result.Data.Title,
                Content = result.Data.Content,
                IsPinned = result.Data.IsPinned,
                CategoryId = result.Data.CategoryId,
                CategoryName = result.Data.CategoryName,
                Tags = result.Data.Tags,
                CreatedAt = result.Data.CreatedAt,
                UpdatedAt = result.Data.UpdatedAt
            };
            
            return Ok(noteDto);
        }
        
        var errors = result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" };
        if (errors.Any(e => e.Contains("not found")))
        {
            return NotFound(errors);
        }
        
        return BadRequest(errors);
    }

    [HttpPost]
    public async Task<ActionResult<NoteDto>> CreateNote([FromBody] CreateNoteDto createNoteDto)
    {
        var command = new CreateNoteCommand(
            createNoteDto.Title,
            createNoteDto.Content,
            createNoteDto.IsPinned,
            createNoteDto.CategoryId,
            createNoteDto.Tags
        );
        
        var result = await _mediator.Send(command);
        
        if (result.IsSuccess && result.Data != null)
        {
            var getResult = await _mediator.Send(new GetNoteByIdQuery(result.Data.Id));
            
            if (getResult.IsSuccess && getResult.Data != null)
            {
                var noteDto = getResult.Data.Adapt<NoteDto>();
                return CreatedAtAction(nameof(GetNote), new { id = noteDto.Id }, noteDto);
            }
        }
        
        return BadRequest(result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NoteDto>> UpdateNote(Guid id, [FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = new UpdateNoteCommand(
            id,
            updateNoteDto.Title,
            updateNoteDto.Content,
            updateNoteDto.IsPinned,
            updateNoteDto.CategoryId,
            updateNoteDto.Tags
        );
        
        var result = await _mediator.Send(command);
        
        if (result.IsSuccess)
        {
            var getResult = await _mediator.Send(new GetNoteByIdQuery(id));
            
            if (getResult.IsSuccess && getResult.Data != null)
            {
                var noteDto = getResult.Data.Adapt<NoteDto>();
                return Ok(noteDto);
            }
        }
        
        var errors = result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" };
        if (errors.Any(e => e.Contains("not found")))
        {
            return NotFound(errors);
        }
        
        return BadRequest(errors);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteNote(Guid id)
    {
        var result = await _mediator.Send(new DeleteNoteCommand(id));
        
        if (result.IsSuccess)
        {
            return NoContent();
        }
        
        var errors = result.Errors.Any() ? result.Errors : new List<string> { result.ErrorMessage ?? "Unknown error" };
        if (errors.Any(e => e.Contains("not found")))
        {
            return NotFound(errors);
        }
        
        return BadRequest(errors);
    }
}
