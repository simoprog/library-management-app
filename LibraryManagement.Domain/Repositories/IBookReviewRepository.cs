using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Repositories;

public interface IBookReviewRepository
{
    Task<BookReview?> GetByIdAsync(ReviewId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<BookReview>> GetBookReviewsByBookIdAsync(BookId bookId, CancellationToken cancellationToken);

    Task<BookReview> AddAsync(BookReview review, CancellationToken cancellationToken);
    

}