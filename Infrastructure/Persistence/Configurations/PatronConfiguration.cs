using library_management_api.Domain.Entities;
using library_management_api.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace library_management_api.Infrastructure.Persistence.Configurations;

public class PatronConfiguration : IEntityTypeConfiguration<Patron>
{
    public void Configure(EntityTypeBuilder<Patron> builder)
    {
        builder.ToTable("Patrons");
        
        // Primary key
        builder.HasKey(p => p.Id);
        
        // PatronId configuration - Value Object conversion
        builder.Property(p => p.PatronId)
            .HasConversion(
                patronId => patronId.Value,
                value => new PatronId(value)
            )
            .HasColumnName("PatronId");
        
        // Basic properties
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.HasIndex(p => p.Email)
            .IsUnique();
        
        // Enum configuration
        builder.Property(p => p.Type)
            .HasConversion<string>()
            .HasMaxLength(20);
            
        // Boolean configuration
        builder.Property(p => p.IsActive)
            .IsRequired();
        
        // Money value object as owned type
        builder.OwnsOne(p => p.OutstandingFees, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("OutstandingFeesAmount")
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasMaxLength(3)
                .HasColumnName("OutstandingFeesCurrency")
                .IsRequired();
        });
        
        // Base entity properties
        builder.Property(p => p.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();
            
        builder.Property(p => p.UpdatedAt)
            .HasColumnType("datetime2")
            .IsRequired(false);
        
        // Ignore domain events
        builder.Ignore(p => p.DomainEvents);
    }
}