using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Services;

public class CanReviewPolicy
{
    public bool CanReview(Book book, Patron patron)
    {
        var patronId = patron.PatronId;
        var currentBorrower = book.CurrentBorrowerId;
        
        if (currentBorrower != patronId)
            return false;
        if (book.IsRestrictedAccess)
            return false;
        
        return true;
    }

    public string ReviewRejectionReason(Book book, Patron patron)
    {
        var patronId = patron.PatronId;
        var currentBorrower = book.CurrentBorrowerId;
        
        if (currentBorrower != patronId)
            return "This patron is not the current borrower !";
        
        if (book.IsRestrictedAccess)
            return "This book has restricted access !";
        
        return "You can't review this book !";
        
    }
}