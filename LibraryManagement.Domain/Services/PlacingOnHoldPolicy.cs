using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Services;

public class PlacingOnHoldPolicy
{
    public bool CanPlaceOnHold(Book book, Patron patron)
    {
        if (!patron.IsActive)
            return false;
            
        if (patron.HasOutstandingFees())
            return false;
            
        if (book.IsRestrictedAccess && !patron.CanHoldRestrictedBooks())
            return false;
            
        return true;
    }
    
    public string GetRejectionReason(Book book, Patron patron)
    {
        if (!patron.IsActive)
            return "Patron account is inactive";
            
        if (patron.HasOutstandingFees())
            return "Patron has outstanding fees";
            
        if (book.IsRestrictedAccess && !patron.CanHoldRestrictedBooks())
            return "Patron cannot access restricted books";
            
        return "Hold request is valid";
    }
}