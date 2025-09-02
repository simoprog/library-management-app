using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Queries.GetBookById;

public record GetBookByIdQuery(Guid BookId) : IRequest<Result<BookDto>>;
