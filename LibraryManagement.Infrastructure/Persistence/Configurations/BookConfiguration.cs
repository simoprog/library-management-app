using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        
        // Primary key configuration
        builder.HasKey(b => b.Id);
        
        // BookId configuration
        builder.Property(b => b.BookId)
            .HasConversion(
                bookId => bookId.Value,
                value => new BookId(value)
            )
            .HasColumnName("BookId");
        // Basic properties
        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(13);
            
        builder.HasIndex(b => b.ISBN)
            .IsUnique();
        
        // Enum configuration
        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
        
        // Value Object configurations
        builder.Property(b => b.CurrentHolderId)
            .HasConversion(
                patronId => patronId != null ? patronId.Value : (Guid?)null,
                value => value.HasValue ? new PatronId(value.Value) : null
            )
            .IsRequired(false);
            
        builder.Property(b => b.CurrentBorrowerId)
            .HasConversion(
                patronId => patronId != null ? patronId.Value : (Guid?)null,
                value => value.HasValue ? new PatronId(value.Value) : null
            )
            .IsRequired(false);
        
        // Date configurations
        builder.Property(b => b.HoldExpiryDate)
            .HasColumnType("datetime2")
            .IsRequired(false);
            
        builder.Property(b => b.CheckoutDate)
            .HasColumnType("datetime2")
            .IsRequired(false);
            
        builder.Property(b => b.DueDate)
            .HasColumnType("datetime2")
            .IsRequired(false);
        
        // Base entity properties
        builder.Property(b => b.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();
            
        builder.Property(b => b.UpdatedAt)
            .HasColumnType("datetime2")
            .IsRequired(false);
        
        // Ignore domain events (not persisted)
        builder.Ignore(b => b.DomainEvents);
    }
}