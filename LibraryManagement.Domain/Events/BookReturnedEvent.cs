using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Events;

public record BookReturnedEvent(
    BookId BookId,
    PatronId PatronId) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}