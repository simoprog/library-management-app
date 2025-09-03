using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Queries.GetPatronHolds;

// You'll implement GetPatronHoldsQuery  Application layer
public record GetPatronHoldsQuery(Guid PatronId) : IRequest<Result<List<BookDto>>>;
