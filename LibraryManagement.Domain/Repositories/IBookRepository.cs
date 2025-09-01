using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Repositories;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(BookId id, CancellationToken cancellationToken = default);
    Task<Book?> GetByISBNAsync(string isbn, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Book>> GetBooksOnHoldForPatronAsync(PatronId patronId, CancellationToken cancellationToken = default);
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
}