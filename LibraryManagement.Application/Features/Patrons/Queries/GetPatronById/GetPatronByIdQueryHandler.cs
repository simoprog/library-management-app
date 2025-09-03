using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Queries.GetPatronById;

public class GetPatronByIdQueryHandler : IRequestHandler<GetPatronByIdQuery ,Result<PatronDto>>
{
        private readonly IPatronRepository _patronRepository;
        
        public GetPatronByIdQueryHandler(IPatronRepository patronRepository)
        {
                _patronRepository = patronRepository;
        }

        public async Task<Result<PatronDto>> Handle(GetPatronByIdQuery request, CancellationToken cancellationToken)
        {
                var patron = await _patronRepository.GetByIdAsync(new PatronId(request.PatronId), cancellationToken);

                if (patron == null)
                        return Result<PatronDto>.Failure("Book not found");
                var patronDto = new PatronDto(
                        patron.Id,
                        patron.PatronId.Value,
                        patron.Name,
                        patron.Email,
                        patron.Type.ToString(),
                        patron.OutstandingFees.Amount,
                        patron.OutstandingFees.Currency,
                        patron.IsActive,
                        patron.CreatedAt);

                return Result<PatronDto>.Success(patronDto);
        }
}
        
