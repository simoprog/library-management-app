using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.Services;
using LibraryManagement.Domain.ValueObjects;
using MediatR;

namespace LibraryManagement.Application.Features.BookReviews.Commands.CreateBookReview;

public class CreateBookCommandHandler : IRequestHandler<CreateBookReviewCommand , Result<BookReviewDto>>
{
    private readonly IBookReviewRepository _bookReviewRepository;
    private readonly IPatronRepository _patronRepository;
    private readonly IBookRepository _bookRepository;
    private readonly CanReviewPolicy _canReviewPolicy;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IBookReviewRepository bookReviewRepository, IBookRepository bookRepository ,IPatronRepository patronRepository, IUnitOfWork unitOfWork , CanReviewPolicy canReviewPolicy)
    {
        _bookReviewRepository = bookReviewRepository;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _canReviewPolicy = canReviewPolicy;
        _patronRepository = patronRepository;
    }
    public async Task<Result<BookReviewDto>> Handle(CreateBookReviewCommand request, CancellationToken cancellationToken)
    {
        var patron = await _patronRepository.GetByIdAsync(request.PatronId, cancellationToken);
        if (patron == null)
            return Result<BookReviewDto>.Failure("This patron doesn't exist !");
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
        if (book == null)
            return Result<BookReviewDto>.Failure("This book doesn't exist");
        if (!_canReviewPolicy.CanReview(book, patron))
        {
            var reason = _canReviewPolicy.ReviewRejectionReason(book, patron);
            return Result<BookReviewDto>.Failure(reason);
        }
        var review = new BookReview(request.BookId, request.PatronId, request.Rating, request.Description);
        await _bookReviewRepository.AddAsync(review, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var reviewDto = new BookReviewDto
        (
            review.Id,
             review.ReviewId,
            request.BookId,
            request.PatronId,
             review.Rating,
          review.Description,
          review.CreatedAt
        );
        return Result<BookReviewDto>.Success(reviewDto) ;
    }
}