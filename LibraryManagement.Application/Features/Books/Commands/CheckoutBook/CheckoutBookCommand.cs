using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.CheckoutBook;

public record CheckoutBookCommand(
    Guid BookId,
    Guid PatronId
) : IRequest<Result>;