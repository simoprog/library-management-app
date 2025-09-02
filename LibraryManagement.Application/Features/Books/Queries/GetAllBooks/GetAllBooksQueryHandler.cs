using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Repositories;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Queries;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, Result<List<BookDto>>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<List<BookDto>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAvailableBooksAsync(cancellationToken);
        
        var bookDtos = books.Select(book => new BookDto(
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
        )).ToList();

        return Result<List<BookDto>>.Success(bookDtos);
    }
}