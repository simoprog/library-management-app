using library_management_api.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace library_management_api.Infrastructure.Persistence.Configurations;

public static class ValueObjectConfigurations
{
    public static void ConfigureBookId(this PropertyBuilder<BookId> builder)
    {
        builder.HasConversion(
            id => id.Value,
            value => new BookId(value)
        );
    }
    
    public static void ConfigurePatronId(this PropertyBuilder<PatronId> builder)
    {
        builder.HasConversion(
            id => id.Value,
            value => new PatronId(value)
        );
    }
    
    public static void ConfigureMoney(this OwnedNavigationBuilder<object, Money> builder)
    {
        builder.Property(m => m.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(m => m.Currency)
            .HasMaxLength(3)
            .IsRequired();
    }
}