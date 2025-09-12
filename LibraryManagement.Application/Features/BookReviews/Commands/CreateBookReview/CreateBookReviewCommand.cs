using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.BookReviews.Commands.CreateBookReview;

public record CreateBookReviewCommand(BookId BookId ,PatronId PatronId , int Rating , string Description) : IRequest<Result<BookReviewDto>>;
