namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

public readonly record struct InvoiceId(int Value)
{
    public static implicit operator int(InvoiceId id) => id.Value;
    public static implicit operator InvoiceId(int value) => new(value);
    public override string ToString() => Value.ToString();
}
