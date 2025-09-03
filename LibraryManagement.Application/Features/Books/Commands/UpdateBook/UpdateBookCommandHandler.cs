using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BookDto>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(new BookId(request.BookId), cancellationToken);
        if (book == null)
        {
            return Result<BookDto>.Failure("Book not found");
        }

        // Check if another book has this ISBN (excluding current book)
        var existingBook = await _bookRepository.GetByISBNAsync(request.ISBN, cancellationToken);
        if (existingBook != null && existingBook.BookId.Value != request.BookId)
        {
            return Result<BookDto>.Failure("Another book with this ISBN already exists");
        }
        
        try
        {
            book.UpdateDetails(
                request.Title,
                request.Author,
                request.ISBN,
                request.IsRestrictedAccess
            );

            await _bookRepository.UpdateAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
        catch (Exception ex)
        {
            return Result<BookDto>.Failure($"Failed to update book: {ex.Message}");
        }
    }
}