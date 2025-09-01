namespace LibraryManagement.Application.DTOs;

public record CreateBookDto(
    string Title,
    string Author,
    string ISBN,
    bool IsRestrictedAccess = false
);