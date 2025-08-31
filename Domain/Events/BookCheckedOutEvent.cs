using library_management_api.Domain.ValueObjects;

namespace library_management_api.Domain.Events;

public record BookCheckedOutEvent(
    BookId BookId,
    PatronId PatronId,
    DateTime DueDate) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
