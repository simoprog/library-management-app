using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Events;

public record BookPlacedOnHoldEvent(
    BookId BookId,
    PatronId PatronId, 
    DateTime ExpiryDate) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
