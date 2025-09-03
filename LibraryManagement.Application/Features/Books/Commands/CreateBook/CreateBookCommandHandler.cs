using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand,Result<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetByISBNAsync(request.ISBN, cancellationToken);
        if (existingBook != null)
        {
            return Result<BookDto>.Failure("A book with this ISBN already exists");
        }
        var book = new Book(request.Title, request.Author, request.ISBN, request.IsRestrictedAccess);
        
        await _bookRepository.AddAsync(book, cancellationToken);
        
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
}