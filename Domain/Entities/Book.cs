using library_management_api.Domain.Common;
using library_management_api.Domain.Enums;
using library_management_api.Domain.Events;
using library_management_api.Domain.ValueObjects;

namespace library_management_api.Domain.Entities;

public class Book : EntityBase
{
    public BookId BookId { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string ISBN { get; private set; }
    public BookStatus Status { get; private set; }
    public bool IsRestrictedAccess { get; private set; }
    public PatronId? CurrentHolderId { get; private set; }
    public PatronId? CurrentBorrowerId { get; private set; }
    public DateTime? HoldExpiryDate { get; private set; }
    public DateTime? CheckoutDate { get; private set; }
    public DateTime? DueDate { get; private set; }
    
    private Book() { } 
    public Book(string title, string author, string isbn, bool isRestrictedAccess = false)
    {
        BookId = BookId.New();
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Author = author ?? throw new ArgumentNullException(nameof(author));
        ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
        Status = BookStatus.Available;
        IsRestrictedAccess = isRestrictedAccess;
    }
    
    public void PlaceOnHold(PatronId patronId, HoldDuration duration)
    {
        if (Status != BookStatus.Available)
            throw new InvalidOperationException($"Book {BookId} is not available for hold");
            
        Status = BookStatus.OnHold;
        CurrentHolderId = patronId;
        HoldExpiryDate = duration.CalculateExpiryFrom(DateTime.UtcNow);
        UpdateTimestamp();
        
        AddDomainEvent(new BookPlacedOnHoldEvent(BookId, patronId, HoldExpiryDate.Value));
    }
    
    public void CheckOut(PatronId patronId)
    {
        if (Status == BookStatus.CheckedOut)
            throw new InvalidOperationException($"Book {BookId} is already checked out");
            
        if (Status == BookStatus.OnHold && CurrentHolderId != patronId)
            throw new InvalidOperationException($"Book {BookId} is on hold for another patron");
            
        Status = BookStatus.CheckedOut;
        CurrentBorrowerId = patronId;
        CurrentHolderId = null;
        HoldExpiryDate = null;
        CheckoutDate = DateTime.UtcNow;
        DueDate = DateTime.UtcNow.AddDays(14); // Standard 2-week checkout
        UpdateTimestamp();
        
        AddDomainEvent(new BookCheckedOutEvent(BookId, patronId, DueDate.Value));
    }
    
    public void Return()
    {
        if (Status != BookStatus.CheckedOut)
            throw new InvalidOperationException($"Book {BookId} is not checked out");
            
        var previousBorrower = CurrentBorrowerId;
        
        Status = BookStatus.Available;
        CurrentBorrowerId = null;
        CheckoutDate = null;
        DueDate = null;
        UpdateTimestamp();
        
        AddDomainEvent(new BookReturnedEvent(BookId, previousBorrower!));
    }
    
    public bool IsOverdue() => Status == BookStatus.CheckedOut && DueDate < DateTime.UtcNow;
    
    public Money CalculateOverdueFee()
    {
        if (!IsOverdue()) return Money.Zero;
        
        var overdueDays = (DateTime.UtcNow - DueDate!.Value).Days;
        return new Money(overdueDays * 0.50m); // $0.50 per day
    }
    
}