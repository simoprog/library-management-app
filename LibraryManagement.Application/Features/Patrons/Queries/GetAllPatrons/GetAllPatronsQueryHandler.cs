using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Repositories;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Queries.GetAllPatrons;

public class GetAllPatronsQueryHandler : IRequestHandler<GetAllPatronsQuery, Result<List<PatronDto>>>
{
    private readonly IPatronRepository _patronRepository;

    public GetAllPatronsQueryHandler(IPatronRepository patronRepository)
    {
        _patronRepository = patronRepository;
    }

    public async Task<Result<List<PatronDto>>> Handle(GetAllPatronsQuery request, CancellationToken cancellationToken)
    {
        var patrons = await _patronRepository.GetActivePatronsAsync(cancellationToken);
        
        var patronDtos = patrons.Select(patron => new PatronDto(
            patron.Id,
            patron.PatronId.Value,
            patron.Name,
            patron.Email,
            patron.Type.ToString(),
            patron.OutstandingFees.Amount,
            patron.OutstandingFees.Currency,
            patron.IsActive,
            patron.CreatedAt
        )).ToList();

        return Result<List<PatronDto>>.Success(patronDtos);
    }
}