namespace LibraryManagement.Application.DTOs;

public record UpdatePatronDto(
    string Name,
    string Email,
    string Type
);