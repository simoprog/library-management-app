using library_management_api.Domain.Entities;
using library_management_api.Domain.Repositories;
using library_management_api.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace library_management_api.Infrastructure.Persistence.Repositories;

public class PatronRepository : IPatronRepository
{
    private readonly ApplicationDbContext _context;
    
    public PatronRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Patron?> GetByIdAsync(PatronId id, CancellationToken cancellationToken = default)
    {
        return await _context.Patrons
            .FirstOrDefaultAsync(p => p.PatronId == id, cancellationToken);
    }
    
    public async Task<Patron?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Patrons
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }
    
    public async Task<IReadOnlyList<Patron>> GetActivePatronsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Patrons
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Patron> AddAsync(Patron patron, CancellationToken cancellationToken = default)
    {
        _context.Patrons.Add(patron);
        return patron;
    }
    
    public async Task UpdateAsync(Patron patron, CancellationToken cancellationToken = default)
    {
        _context.Entry(patron).State = EntityState.Modified;
    }
    
    public async Task DeleteAsync(Patron patron, CancellationToken cancellationToken = default)
    {
        _context.Patrons.Remove(patron);
    }
}