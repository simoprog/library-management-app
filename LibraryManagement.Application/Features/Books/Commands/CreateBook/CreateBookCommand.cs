using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.CreateBook;

public record CreateBookCommand(
    string Title,
    string Author,
    string ISBN,
    bool IsRestrictedAccess = false
) : IRequest<Result<BookDto>>;
