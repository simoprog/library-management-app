namespace LibraryManagement.Domain.ValueObjects;

public record ReviewId(Guid Value)
{
    public static ReviewId New() => new(Guid.NewGuid());
    public static ReviewId From(string Value) => new(Guid.Parse(Value));

    public override string ToString()
    {
        return Value.ToString(); 
    }
}