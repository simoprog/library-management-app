using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Commands.DeleteBook;

public record DeleteBookCommand(Guid BookId) : IRequest<Result>;
