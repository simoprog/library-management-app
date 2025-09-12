using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public class BookReviewRepository : IBookReviewRepository
{
    private readonly ApplicationDbContext _context;

    public BookReviewRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<BookReview?> GetByIdAsync(ReviewId id, CancellationToken cancellationToken = default)
    {
        return await _context.BookReviews.FirstOrDefaultAsync(r => r.ReviewId == id ,cancellationToken);
    }

    public async Task<BookReview> AddAsync(BookReview bookReview, CancellationToken cancellationToken)
    {
         _context.BookReviews.Add(bookReview);
          return bookReview;
    }

    public async Task<IReadOnlyList<BookReview>> GetBookReviewsByBookIdAsync(BookId bookId,
        CancellationToken cancellationToken)
    {
        return await _context.BookReviews.Where(r => r.BookId == bookId.Value).ToListAsync(cancellationToken);
    }


}