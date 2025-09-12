using System.ComponentModel.DataAnnotations;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Entities;

public class BookReview : EntityBase
{
    public ReviewId ReviewId { get; set; }
    public Guid BookId { get; set; }
    public Guid PatronId { get; set; }

    
    [Range(1,5)]
    public int Rating { get; set; }
    
    public string Description { get; set; }
    public Book Book { get; set; }
    public Patron Patron { get; set; }
    public BookReview(BookId bookId, PatronId patronId, int rating, string description)
    {
        ReviewId = ReviewId.New();
        BookId = bookId.Value;
        PatronId = patronId.Value;
        Rating = rating;
        Description = description;
    }

  
}