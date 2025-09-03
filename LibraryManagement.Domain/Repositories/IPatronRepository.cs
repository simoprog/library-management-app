using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Repositories;

public interface IPatronRepository
{
    Task<Patron?> GetByIdAsync(PatronId id, CancellationToken cancellationToken = default);
    Task<Patron?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Patron>> GetPatronsAsync(CancellationToken cancellationToken = default);
    Task<Patron> AddAsync(Patron patron, CancellationToken cancellationToken = default);
    Task UpdateAsync(Patron patron, CancellationToken cancellationToken = default);
    Task DeleteAsync(Patron patron, CancellationToken cancellationToken = default);
}