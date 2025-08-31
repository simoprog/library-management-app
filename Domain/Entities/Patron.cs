using library_management_api.Domain.Common;
using library_management_api.Domain.Enums;
using library_management_api.Domain.ValueObjects;

namespace library_management_api.Domain.Entities;

public class Patron : EntityBase
{
    public PatronId PatronId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public PatronType Type { get; private set; }
    public Money OutstandingFees { get; private set; }
    public bool IsActive { get; private set; }
    
    private Patron() { } 
    
    public Patron(string name, string email, PatronType type)
    {
        PatronId = PatronId.New();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Type = type;
        OutstandingFees = Money.Zero;
        IsActive = true;
    }
    
    public bool CanHoldRestrictedBooks() => Type == PatronType.Researcher;
    
    public bool HasOutstandingFees() => OutstandingFees.Amount > 0;
    
    public void AddFee(Money fee)
    {
        OutstandingFees += fee;
        UpdateTimestamp();
    }
    
    public void PayFee(Money amount)
    {
        if (amount.Amount <= 0)
            throw new ArgumentException("Payment amount must be positive");
            
        if (amount.Amount > OutstandingFees.Amount)
            throw new ArgumentException("Payment exceeds outstanding fees");
            
        OutstandingFees = new Money(OutstandingFees.Amount - amount.Amount);
        UpdateTimestamp();
    }
    
    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }
}