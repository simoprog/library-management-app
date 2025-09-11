using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Configurations;

public class BookReviewConfiguration : IEntityTypeConfiguration<BookReview>
{
    public void Configure(EntityTypeBuilder<BookReview> builder)
    {
        builder.ToTable("BookReviews");
        
        // Add Constraints
        builder.ToTable(br =>
            br.HasCheckConstraint("CK_BOOK-REVIEW_RatingRange", "Rating >=1 AND Rating <=5"));
        
        //Primary Key
        builder.HasKey(br=>br.Id);

        builder.Property(br => br.ReviewId).HasConversion(
            reviewId => reviewId.Value,
            value => new ReviewId(value)
        );
        
        builder.Property(br => br.Rating).IsRequired();
        builder.Property(br => br.Description).HasMaxLength(200).IsRequired();
        builder.HasOne(br => br.Book).WithOne();
        builder.HasOne(br => br.Patron).WithOne();
       

        
        

    }
}