namespace RentalPlatform.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "COP")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative");

        Amount = amount;
        Currency = currency.ToUpper();
    }

    public Money Multiply(int factor) => new(Amount * factor, Currency);

    public override string ToString() => $"{Currency} {Amount:N2}";
}
