using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Features.Books.Commands.CheckoutBook;
using LibraryManagement.Application.Features.Books.Commands.CreateBook;
using LibraryManagement.Application.Features.Books.Commands.DeleteBook;
using LibraryManagement.Application.Features.Books.Commands.PlaceBookOnHold;
using LibraryManagement.Application.Features.Books.Commands.UpdateBook;
using LibraryManagement.Application.Features.Books.Queries;
using LibraryManagement.Application.Features.Books.Queries.GetBookById;
using Microsoft.AspNetCore.Mvc;

namespace library_management_api.Controllers;

public class BooksController : BaseApiController
{
    /// <summary>
    /// Get all available books
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BookDto>>> GetAllBooks()
    {
        var result = await Mediator.Send(new GetAllBooksQuery());
        return HandleResult(result);
    }

    /// <summary>
    /// Get a specific book by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> GetBookById(Guid id)
    {
        var result = await Mediator.Send(new GetBookByIdQuery(id));
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new book
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        var command = new CreateBookCommand(
            createBookDto.Title,
            createBookDto.Author,
            createBookDto.ISBN,
            createBookDto.IsRestrictedAccess);

        var result = await Mediator.Send(command);
        
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetBookById), new { id = result.Value.BookId }, result.Value);
            
        return HandleResult(result);
    }
    /// <summary>
    /// Update an existing book
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> UpdateBook(Guid id, [FromBody] UpdateBookDto updateBookDto)
    {
        var command = new UpdateBookCommand(
            id,
            updateBookDto.Title,
            updateBookDto.Author,
            updateBookDto.ISBN,
            updateBookDto.IsRestrictedAccess);

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
    
    /// <summary>
    /// Delete a book
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteBook(Guid id)
    {
        var command = new DeleteBookCommand(id);
        var result = await Mediator.Send(command);
    
        if (result.IsSuccess)
            return NoContent();
        
        return HandleResult(result);
    }
    
    /// <summary>
    /// Place a book on hold
    /// </summary>
    [HttpPost("{bookId:guid}/hold")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PlaceBookOnHold(Guid bookId, [FromBody] PlaceHoldRequest request)
    {
        var command = new PlaceBookOnHoldCommand(bookId, request.PatronId);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Check out a book
    /// </summary>
    [HttpPost("{bookId:guid}/checkout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CheckoutBook(Guid bookId, [FromBody] CheckoutRequest request)
    {
        var command = new CheckoutBookCommand(bookId, request.PatronId);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Return a book
    /// </summary>
    [HttpPost("{bookId:guid}/return")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ReturnBook(Guid bookId)
    {
        // You'll need to implement ReturnBookCommand in Application layer
        // For now, return NotImplemented
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
}

public record PlaceHoldRequest(Guid PatronId);
public record CheckoutRequest(Guid PatronId);