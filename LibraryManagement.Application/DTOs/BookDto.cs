namespace LibraryManagement.Application.DTOs;

public record BookDto(
    Guid Id,
    Guid BookId,
    string Title,
    string Author,
    string ISBN,
    string Status,
    bool IsRestrictedAccess,
    Guid? CurrentHolderId,
    Guid? CurrentBorrowerId,
    DateTime? HoldExpiryDate,
    DateTime? DueDate,
    DateTime CreatedAt
);