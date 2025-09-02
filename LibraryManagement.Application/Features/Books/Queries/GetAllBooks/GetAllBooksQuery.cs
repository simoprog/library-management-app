using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Features.Books.Queries;

public record GetAllBooksQuery() : IRequest<Result<List<BookDto>>>;
