﻿using MediatR;

namespace LibraryManagement.Domain.Events;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
    
}