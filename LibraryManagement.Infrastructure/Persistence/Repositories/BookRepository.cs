using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using LibraryManagementInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;
    
    public BookRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Book?> GetByIdAsync(BookId id, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == id, cancellationToken);
    }
    
    public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .FirstOrDefaultAsync(b => b.ISBN == isbn, cancellationToken);
    }
    
    public async Task<IReadOnlyList<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(b => b.Status == BookStatus.Available)
            .OrderBy(b => b.Title)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<Book>> GetBooksOnHoldForPatronAsync(PatronId patronId, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(b => b.Status == BookStatus.OnHold && b.CurrentHolderId == patronId)
            .OrderBy(b => b.HoldExpiryDate)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
         _context.Books.Add(book);
        return book;
    }
    
    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Entry(book).State = EntityState.Modified;
    }
    
    public async Task DeleteAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Books.Remove(book);
    }
}