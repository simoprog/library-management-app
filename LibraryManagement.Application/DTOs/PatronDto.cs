namespace LibraryManagement.Application.DTOs;

public record PatronDto(
    Guid Id,
    Guid PatronId,
    string Name,
    string Email,
    string Type,
    decimal OutstandingFeesAmount,
    string OutstandingFeesCurrency,
    bool IsActive,
    DateTime CreatedAt
);