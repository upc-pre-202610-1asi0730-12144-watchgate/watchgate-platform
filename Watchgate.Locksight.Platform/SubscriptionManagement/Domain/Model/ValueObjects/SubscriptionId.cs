namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

public readonly record struct SubscriptionId(int Value)
{
    public static implicit operator int(SubscriptionId id) => id.Value;
    public static implicit operator SubscriptionId(int value) => new(value);
    public override string ToString() => Value.ToString();
}