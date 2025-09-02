using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<BookDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(new BookId(request.BookId), cancellationToken);
        
        if (book == null)
            return Result<BookDto>.Failure("Book not found");

        var bookDto = new BookDto(
            book.Id,
            book.BookId.Value,
            book.Title,
            book.Author,
            book.ISBN,
            book.Status.ToString(),
            book.IsRestrictedAccess,
            book.CurrentHolderId?.Value,
            book.CurrentBorrowerId?.Value,
            book.HoldExpiryDate,
            book.DueDate,
            book.CreatedAt
        );

        return Result<BookDto>.Success(bookDto);
    }
}