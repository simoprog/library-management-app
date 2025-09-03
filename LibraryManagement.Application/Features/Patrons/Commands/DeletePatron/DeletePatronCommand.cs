using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Features.Patrons.Commands.DeletePatron;

public record DeletePatronCommand(Guid PatronId) : IRequest<Result>;