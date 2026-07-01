using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Errors;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

public sealed record CardNumber
{
    public string Value { get; }

    public CardNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 16 || !value.All(char.IsDigit))
            throw new ArgumentException(SubscriptionErrors.InvalidCardNumber.Message, nameof(value));
            
        Value = value;
    }

    public static implicit operator string(CardNumber cardNumber) => cardNumber.Value;
    public static implicit operator CardNumber(string value) => new(value);

    public override string ToString() => Value;
}