using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

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
    
    public async Task<IReadOnlyList<Patron>> GetPatronsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Patrons
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