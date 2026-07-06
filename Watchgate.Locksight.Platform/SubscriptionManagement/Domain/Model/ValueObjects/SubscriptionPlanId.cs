namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

public readonly record struct SubscriptionPlanId(int Value)
{
    public static implicit operator int(SubscriptionPlanId id) => id.Value;
    public static implicit operator SubscriptionPlanId(int value) => new(value);
    public override string ToString() => Value.ToString();
}