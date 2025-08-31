using MediatR;

namespace library_management_api.Domain.Events;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
    
}