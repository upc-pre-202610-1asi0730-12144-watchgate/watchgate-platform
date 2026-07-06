namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

public readonly record struct PaymentId(int Value)
{
    public static implicit operator int(PaymentId id) => id.Value;
    public static implicit operator PaymentId(int value) => new(value);
    public override string ToString() => Value.ToString();
}
