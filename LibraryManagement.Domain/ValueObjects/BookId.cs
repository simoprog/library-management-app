namespace LibraryManagement.Domain.ValueObjects;

public record BookId(Guid Value)
{
    public static BookId New() => new(Guid.NewGuid());
    public static BookId From(string value) => new(Guid.Parse(value));
    
    public override string ToString() => Value.ToString();
}