using System.Text.RegularExpressions;

namespace RentalPlatform.Domain.ValueObjects;

public sealed class Email
{
    private const string Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, Pattern))
            throw new ArgumentException("Invalid email format");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
