namespace LibraryManagement.Application.DTOs;

public record CreatePatronDto(
    string Name,
    string Email,
    string Type
);