using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Features.Patrons.Commands.CreatePatron;
using LibraryManagement.Application.Features.Patrons.Commands.DeletePatron;
using LibraryManagement.Application.Features.Patrons.Commands.UpdatePatron;
using LibraryManagement.Application.Features.Patrons.Queries.GetAllPatrons;
using LibraryManagement.Application.Features.Patrons.Queries.GetPatronById;
using LibraryManagement.Application.Features.Patrons.Queries.GetPatronHolds;
using Microsoft.AspNetCore.Mvc;

namespace library_management_api.Controllers;


public class PatronsController : BaseApiController
{
    /// <summary>
    /// Get all active patrons
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PatronDto>>> GetAllPatrons()
    {
        var result = await Mediator.Send(new GetAllPatronsQuery());
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new patron
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatronDto>> CreatePatron([FromBody] CreatePatronDto createPatronDto)
    {
        var command = new CreatePatronCommand(
            createPatronDto.Name,
            createPatronDto.Email,
            createPatronDto.Type);

        var result = await Mediator.Send(command);
        
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetPatronById), new { id = result.Value.PatronId }, result.Value);
            
        return HandleResult(result);
    }

    /// <summary>
    /// Get patron by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatronDto>> GetPatronById(Guid id)
    {
        var result = await Mediator.Send(new GetPatronByIdQuery(id));
        return HandleResult(result);
    }
    
    /// <summary>
    /// Update an existing patron
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatronDto>> UpdatePatron(Guid id, [FromBody] UpdatePatronDto updatePatronDto)
    {
        var command = new UpdatePatronCommand(
            id,
            updatePatronDto.Name,
            updatePatronDto.Email,
            updatePatronDto.Type);

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete/Deactivate a patron
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePatron(Guid id)
    {
        var command = new DeletePatronCommand(id);
        var result = await Mediator.Send(command);
    
        if (result.IsSuccess)
            return NoContent();
        
        return HandleResult(result);
    }

    /// <summary>
    /// Get books on hold for a patron
    /// </summary>
    [HttpGet("{patronId:guid}/holds")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<BookDto>>> GetPatronHolds(Guid patronId)
    {
        var result = await Mediator.Send(new GetPatronHoldsQuery(patronId));
        return HandleResult(result);
    }
}