using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Commands.UpdatePatron;


public record UpdatePatronCommand(
    Guid PatronId,
    string Name,
    string Email,
    string Type
) : IRequest<Result<PatronDto>>;