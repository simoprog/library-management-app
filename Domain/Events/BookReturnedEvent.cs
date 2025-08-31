using library_management_api.Domain.ValueObjects;

namespace library_management_api.Domain.Events;

public record BookReturnedEvent(
    BookId BookId,
    PatronId PatronId) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}