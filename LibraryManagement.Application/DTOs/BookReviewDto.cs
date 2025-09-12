using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.DTOs;

public record BookReviewDto(
    Guid Id,
    ReviewId ReviewId,
    BookId BookId,
    PatronId PatronId,
    int Rating,
    string Description,
    DateTime CreatedAt
    );