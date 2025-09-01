using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.PlaceBookOnHold;

public record PlaceBookOnHoldCommand(
    Guid BookId,
    Guid PatronId
) : IRequest<Result>;