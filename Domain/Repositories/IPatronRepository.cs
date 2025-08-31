using library_management_api.Domain.Entities;
using library_management_api.Domain.ValueObjects;

namespace library_management_api.Domain.Repositories;

public interface IPatronRepository
{
    Task<Patron?> GetByIdAsync(PatronId id, CancellationToken cancellationToken = default);
    Task<Patron?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Patron>> GetActivePatronsAsync(CancellationToken cancellationToken = default);
    Task<Patron> AddAsync(Patron patron, CancellationToken cancellationToken = default);
    Task UpdateAsync(Patron patron, CancellationToken cancellationToken = default);
    Task DeleteAsync(Patron patron, CancellationToken cancellationToken = default);
}