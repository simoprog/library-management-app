using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Commands.CreatePatron;

public record CreatePatronCommand(
    string Name,
    string Email,
    string Type
) : IRequest<Result<PatronDto>>;
