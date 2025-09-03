using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(new BookId(request.BookId), cancellationToken);
        if (book == null)
        {
            return Result.Failure("Book not found");
        }

        // Business rule: Cannot delete books that are currently on hold or checked out
        if (book.Status == BookStatus.OnHold)
        {
            return Result.Failure("Cannot delete book that is currently on hold");
        }

        if (book.Status == BookStatus.CheckedOut)
        {
            return Result.Failure("Cannot delete book that is currently checked out");
        }

        try
        {
            await _bookRepository.DeleteAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete book: {ex.Message}");
        }
    }
}
