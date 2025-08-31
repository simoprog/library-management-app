using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Patron> Patrons => Set<Patron>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply configurations
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new PatronConfiguration());
        
        // Global query filters
        modelBuilder.Entity<Patron>()
            .HasQueryFilter(p => p.IsActive);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback connection string for design-time
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=LibraryManagementDB;User=sa;Password123;TrustServerCertificate=true;");
        }
    }
}