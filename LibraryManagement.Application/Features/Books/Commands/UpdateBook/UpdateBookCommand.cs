using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    Guid BookId,
    string Title,
    string Author,
    string ISBN,
    bool IsRestrictedAccess
) : IRequest<Result<BookDto>>;