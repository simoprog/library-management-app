namespace LibraryManagement.Domain.ValueObjects;

public record HoldDuration(int Days)
{
    public static HoldDuration Standard => new(7);
    public static HoldDuration Extended => new(14);
    
    public DateTime CalculateExpiryFrom(DateTime startDate) => startDate.AddDays(Days);
}