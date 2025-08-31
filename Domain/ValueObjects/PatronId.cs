namespace library_management_api.Domain.ValueObjects;

public record PatronId(Guid Value)
{
    public static PatronId New() => new(Guid.NewGuid());
    public static PatronId From(string value) => new(Guid.Parse(value));
    
    public override string ToString() => Value.ToString();
}