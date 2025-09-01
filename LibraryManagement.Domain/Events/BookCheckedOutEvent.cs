using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Events;

public record BookCheckedOutEvent(
    BookId BookId,
    PatronId PatronId,
    DateTime DueDate) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
