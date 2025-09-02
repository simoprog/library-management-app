using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Features.Patrons.Commands.CreatePatron;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Repositories;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Commands.CreatePatron;

public class CreatePatronCommandHandler : IRequestHandler<CreatePatronCommand, Result<PatronDto>>
{
    private readonly IPatronRepository _patronRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePatronCommandHandler(IPatronRepository patronRepository, IUnitOfWork unitOfWork)
    {
        _patronRepository = patronRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PatronDto>> Handle(CreatePatronCommand request, CancellationToken cancellationToken)
    {
        // Check if patron with email already exists
        var existingPatron = await _patronRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingPatron != null)
        {
            return Result<PatronDto>.Failure("A patron with this email already exists");
        }

        if (!Enum.TryParse<PatronType>(request.Type, true, out var patronType))
        {
            return Result<PatronDto>.Failure("Invalid patron type. Must be 'Regular' or 'Researcher'");
        }

        var patron = new Patron(request.Name, request.Email, patronType);
        
        await _patronRepository.AddAsync(patron, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return DTO
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
}