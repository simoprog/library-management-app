using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.Services;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.PlaceBookOnHold;

public class PlaceBookOnHoldCommandHandler : IRequestHandler<PlaceBookOnHoldCommand, Result>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private readonly PlacingOnHoldPolicy _holdPolicy;
    private readonly IUnitOfWork _unitOfWork;

    public PlaceBookOnHoldCommandHandler(
        IBookRepository bookRepository,
        IPatronRepository patronRepository,
        PlacingOnHoldPolicy holdPolicy,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
        _holdPolicy = holdPolicy;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PlaceBookOnHoldCommand request, CancellationToken cancellationToken)
    {
        // Get book and patron
        var book = await _bookRepository.GetByIdAsync(new BookId(request.BookId), cancellationToken);
        if (book == null)
            return Result.Failure("Book not found");

        var patron = await _patronRepository.GetByIdAsync(new PatronId(request.PatronId), cancellationToken);
        if (patron == null)
            return Result.Failure("Patron not found");

        // Check business rules
        if (!_holdPolicy.CanPlaceOnHold(book, patron))
        {
            return Result.Failure(_holdPolicy.GetRejectionReason(book, patron));
        }

        // Place hold
        try
        {
            book.PlaceOnHold(patron.PatronId, HoldDuration.Standard);
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