using Edemo.Domain.Common;

namespace Edemo.Domain.TopUp.ValueObjects;

public class Nickname : ValueObject
{
    public string Value { get;private set; }

    private Nickname()
    {
    }
    private Nickname(string value)
    {
        Value = value;
    }

    public static Nickname Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Nickname cannot be empty.");

        if (value.Length > 20)
            throw new ArgumentException("Nickname must be 20 characters or fewer.");

        return new Nickname(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
    
    public static implicit operator string(Nickname nickname)
    {
        return nickname.ToString();
    }
}