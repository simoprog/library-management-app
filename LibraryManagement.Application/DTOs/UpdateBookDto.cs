namespace LibraryManagement.Application.DTOs;

public record UpdateBookDto(
    string Title,
    string Author,
    string ISBN,
    bool IsRestrictedAccess
);