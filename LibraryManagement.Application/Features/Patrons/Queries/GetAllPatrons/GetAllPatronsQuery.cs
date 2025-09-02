using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Queries.GetAllPatrons;

public record GetAllPatronsQuery() : IRequest<Result<List<PatronDto>>>;
