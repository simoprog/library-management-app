using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Queries.GetPatronById;

public record GetPatronByIdQuery(Guid PatronId) : IRequest<Result<PatronDto>>;