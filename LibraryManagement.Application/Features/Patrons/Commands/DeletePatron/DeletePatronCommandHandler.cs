using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Commands.DeletePatron;

public class DeletePatronCommandHandler : IRequestHandler<DeletePatronCommand, Result>
{
    private readonly IPatronRepository _patronRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePatronCommandHandler(
        IPatronRepository patronRepository, 
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork)
    {
        _patronRepository = patronRepository;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePatronCommand request, CancellationToken cancellationToken)
    {
        var patron = await _patronRepository.GetByIdAsync(new PatronId(request.PatronId), cancellationToken);
        if (patron == null)
        {
            return Result.Failure("Patron not found");
        }

        // Business rule: Cannot delete patrons with outstanding fees
        if (patron.HasOutstandingFees())
        {
            return Result.Failure("Cannot delete patron with outstanding fees");
        }

        // Business rule: Cannot delete patrons with books on hold or checked out
        var booksOnHold = await _bookRepository.GetBooksOnHoldForPatronAsync(patron.PatronId, cancellationToken);
        if (booksOnHold.Any())
        {
            return Result.Failure("Cannot delete patron with books on hold or checked out");
        }

        try
        {
            // Soft delete - deactivate instead of hard delete
            patron.Deactivate();
            await _patronRepository.UpdateAsync(patron, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete patron: {ex.Message}");
        }
    }
}