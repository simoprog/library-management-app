using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Commands.UpdatePatron;

public class UpdatePatronCommandHandler : IRequestHandler<UpdatePatronCommand, Result<PatronDto>>
{
    private readonly IPatronRepository _patronRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePatronCommandHandler(IPatronRepository patronRepository, IUnitOfWork unitOfWork)
    {
        _patronRepository = patronRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PatronDto>> Handle(UpdatePatronCommand request, CancellationToken cancellationToken)
    {
        var patron = await _patronRepository.GetByIdAsync(new PatronId(request.PatronId), cancellationToken);
        if (patron == null)
        {
            return Result<PatronDto>.Failure("Patron not found");
        }

        // Check if another patron has this email (excluding current patron)
        var existingPatron = await _patronRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingPatron != null && existingPatron.PatronId.Value != request.PatronId)
        {
            return Result<PatronDto>.Failure("Another patron with this email already exists");
        }

        // Parse patron type
        if (!Enum.TryParse<PatronType>(request.Type, true, out var patronType))
        {
            return Result<PatronDto>.Failure("Invalid patron type. Must be 'Regular' or 'Researcher'");
        }

        try
        {
            
            await _patronRepository.UpdateAsync(patron, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var patronDto = new PatronDto(
                patron.Id,
                patron.PatronId.Value,
                patron.Name,
                patron.Email,
                patron.Type.ToString(),
                patron.OutstandingFees.Amount,
                patron.OutstandingFees.Currency,
                patron.IsActive,
                patron.CreatedAt
            );

            return Result<PatronDto>.Success(patronDto);
        }
        catch (Exception ex)
        {
            return Result<PatronDto>.Failure($"Failed to update patron: {ex.Message}");
        }
    }
}