using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.CheckoutBook;

public class CheckoutBookCommandHandler : IRequestHandler<CheckoutBookCommand, Result>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutBookCommandHandler(
        IBookRepository bookRepository,
        IPatronRepository patronRepository,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CheckoutBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(new BookId(request.BookId), cancellationToken);
        if (book == null)
            return Result.Failure("Book not found !");

        var patron = await _patronRepository.GetByIdAsync(new PatronId(request.PatronId), cancellationToken);
        if (patron == null)
            return Result.Failure("Patron not found !");

        if (patron.HasOutstandingFees())
            return Result.Failure("Patron has outstanding fees that must be paid before checkout !");

        try
        {
            book.CheckOut(patron.PatronId);
            await _bookRepository.UpdateAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}