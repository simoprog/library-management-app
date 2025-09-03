using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Queries.GetPatronHolds;

public class GetPatronHoldsQueryHandler : IRequestHandler<GetPatronHoldsQuery, Result<List<BookDto>>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;

    public GetPatronHoldsQueryHandler(IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public async Task<Result<List<BookDto>>> Handle(GetPatronHoldsQuery request, CancellationToken cancellationToken)
    {
        
        var patron = await _patronRepository.GetByIdAsync(new PatronId(request.PatronId), cancellationToken);
        if (patron == null)
        {
            return Result<List<BookDto>>.Failure("Patron not found");
        }

        var booksOnHold = await _bookRepository.GetBooksOnHoldForPatronAsync(patron.PatronId, cancellationToken);
        
        var bookDtos = booksOnHold.Select(book => new BookDto(
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